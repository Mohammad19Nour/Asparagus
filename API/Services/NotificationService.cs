using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Interfaces;
using AsparagusN.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<PresenceHub> _presenceHub;
    private readonly PresenceTracker _tracker;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(IHubContext<PresenceHub> presenceHub, PresenceTracker tracker, IUnitOfWork unitOfWork)
    {
        _presenceHub = presenceHub;
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
                IsSent = false
            };
            var connections = await _tracker.GetConnectionsForUser(userEmail);
            if (connections != null)
            {
                await _presenceHub.Clients.Clients(connections)
                    .SendAsync("ReceiveNotification", notification);
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
}