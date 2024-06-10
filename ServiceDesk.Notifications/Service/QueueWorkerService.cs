
using Microsoft.AspNetCore.SignalR;
using ServiceDesk.Util;

namespace ServiceDesk.Notifications.Service;

public class QueueWorkerService(ILogger<QueueWorkerService> logger, QueueService queue, IHubContext<NotificationHub, INotification> notificationHub) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await queue.GetMessageAsync();

            if (message != null)
            {
                logger.LogInformation("Received message - {Message}", message);

                await notificationHub.Clients.All.Notify(message);
            }
        }

        logger.LogInformation("Closing queue worker");
    }
}
