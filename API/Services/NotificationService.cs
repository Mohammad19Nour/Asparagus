using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.Interfaces;
using AsparagusN.SignalR;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class NotificationService : INotificationService
{
    private readonly IMapper _mapper;
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly PresenceTracker _tracker;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(IMapper mapper,IHubContext<NotificationHub> notificationHub, PresenceTracker tracker,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _notificationHub = notificationHub;
        _tracker = tracker;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> NotifyUserByEmail(string userEmail, string arabicContent, string englishContent)
    {
        try
        {
            var notification = new Notification
            {
                ArabicContent = arabicContent,
                EnglishContent = englishContent,
                CreatedAt = DateTime.UtcNow,
                IsSent = false,
                UserEmail = userEmail
            };
            var connections = await _tracker.GetConnectionsForUser(userEmail);
            if (connections.Count > 0)
            {
                var result = _mapper.Map<NotificationDto>(notification);
               
                await _notificationHub.Clients.Clients(connections)
                    .SendAsync(Constants.NewNotificationEventName, new List<NotificationDto> { result });
                notification.IsSent = true;
            }

            _unitOfWork.Repository<Notification>().Add(notification);
            await _unitOfWork.SaveChanges();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> NotifyAllMealPlanUsers(string arabicContent, string englishContent)
    {
        List<string> userEmails = await _unitOfWork.Repository<AppUser>().GetQueryable().Where(x => x.IsMealPlanMember)
            .Select(x => x.Email.ToLower()).ToListAsync();

        foreach (var email in userEmails)
        {
            var ok = await NotifyUserByEmail(email, arabicContent, englishContent);
            if (!ok) return false;
        }

        return true;
    }

    public async Task<bool> NotifyAllNormalUsers(string arabicContent, string englishContent)
    {
        List<string> userEmails = await _unitOfWork.Repository<AppUser>().GetQueryable().Where(x => x.IsNormalUser)
            .Select(x => x.Email.ToLower()).ToListAsync();
        foreach (var email in userEmails)
        {
            var ok = await NotifyUserByEmail(email, arabicContent, englishContent);
            if (!ok) return false;
        }

        return true;
    }

    public async Task<List<Notification>> GetUnsentNotificationsForUserAsync(string userEmail)
    {
        var spec = new UnsentNotificationsForUserSpecification(userEmail);
        var notifications = await _unitOfWork.Repository<Notification>().ListWithSpecAsync(spec);
        return notifications.ToList();
    }

    public async Task<List<Notification>> GetAllNotificationsForUserAsync(string userEmail)
    {
        var notifications = await _unitOfWork.Repository<Notification>().GetQueryable()
            .Where(x => x.UserEmail.ToLower() == userEmail.ToLower()).OrderByDescending(x => x.CreatedAt).ToListAsync();
        notifications = notifications.OrderByDescending(x => x.CreatedAt).ToList();
        return notifications.ToList();
    }

    public async Task<bool> MarkSent(List<Notification> notifications)
    {
        foreach (var notification in notifications)
        {
            notification.IsSent = true;
            _unitOfWork.Repository<Notification>().Update(notification);
        }

        await _unitOfWork.SaveChanges();
        return true;
    }
}