using AsparagusN.DTOs;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AsparagusN.SignalR;

[Authorize]
public class NotificationHub : Hub
{
    private readonly PresenceTracker _tracker;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public NotificationHub(PresenceTracker tracker, INotificationService notificationService,IMapper mapper)
    {
        _tracker = tracker;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public override async Task OnConnectedAsync()
    {
        var email = "";
        if (Context.User != null)
        {
             email = Context.User.GetEmail();
        }
        else
        {
            throw new HubException("UnAuthorized");
        }
        if (email == null)   throw new HubException("UnAuthorized");

        await _tracker.UserConnected(email,Context.ConnectionId);
        // send notifiaction to all users except ont who connected
       // await Clients.Others.SendAsync("UserIsOnline", Context.User.GetEmail());

        await Clients.Caller.SendAsync(Constants.NewNotificationEventName, await getNewNotifications(email)); // method is an event name to listen on the client
    }

    private async Task<List<NotificationDto>> getNewNotifications(string email)
    {
      
        var nots = await _notificationService.GetUnsentNotificationsForUserAsync(email);
        await _notificationService.MarkSent(nots);
        return _mapper.Map<List<NotificationDto>>(nots);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        
        var email = "";
        if (Context.User != null)
        {
            email = Context.User.GetEmail();
        }
        else
        {
            throw new HubException("UnAuthorized");
        }
        if (email == null)   throw new HubException("UnAuthorized");
        await _tracker.UserDisconnected(Context.User.GetEmail(),Context.ConnectionId);
        // send notifiaction to all users except ont who connected
     //   await Clients.Others.SendAsync("UserIsOffline", Context.User.GetEmail());
      //  await Clients.All.SendAsync("GetOnlineUsers", "ok tested");
        await base.OnDisconnectedAsync(exception);
    }
    
}