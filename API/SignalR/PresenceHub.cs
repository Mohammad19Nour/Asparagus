using AsparagusN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AsparagusN.SignalR;

[Authorize]
public class PresenceHub : Hub
{
    private readonly PresenceTracker _tracker;

    public PresenceHub(PresenceTracker tracker)
    {
        _tracker = tracker;
    }

    public override async Task OnConnectedAsync()
    {
        _tracker.UserConnected(Context.User.GetEmail(),Context.ConnectionId);
        // send notifiaction to all users except ont who connected
        await Clients.Others.SendAsync("UserIsOnline", Context.User.GetEmail());

        await Clients.All.SendAsync("GetOnlineUsers", "ok tested");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        
        _tracker.UserDisconnected(Context.User.GetEmail(),Context.ConnectionId);
        // send notifiaction to all users except ont who connected
        await Clients.Others.SendAsync("UserIsOffline", Context.User.GetEmail());
        await Clients.All.SendAsync("GetOnlineUsers", "ok tested");
        await base.OnDisconnectedAsync(exception);
    }
}