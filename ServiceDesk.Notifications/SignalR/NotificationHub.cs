using Microsoft.AspNetCore.SignalR;

namespace ServiceDesk.Notifications;

public class NotificationHub : Hub<INotification>
{
    public async Task NotifyClient(string message)
    {
        await Clients.All.Notify(message);
    }
}
