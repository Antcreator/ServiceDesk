
using ServiceDesk.Util;

namespace ServiceDesk.Notifications.Service;

public class QueueWorkerService(ILogger<QueueWorkerService> logger, QueueService queue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await queue.GetMessageAsync();

            if (message != null)
            {
                logger.LogInformation("Received message - {Message}", message);
            }
        }

        logger.LogInformation("Closing queue worker");
    }
}
