using AsparagusN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AsparagusN.SignalR;

[Authorize]
public class PresenceHub : Hub
{
    
    public override async Task OnConnectedAsync()
    {
        // send notifiaction to all users except ont who connected
        await Clients.Others.SendAsync("UserIsOnline", Context.User.GetEmail());    
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // send notifiaction to all users except ont who connected
        await Clients.Others.SendAsync("UserIsOnline", Context.User.GetEmail());
        await base.OnDisconnectedAsync(exception);
    }
}