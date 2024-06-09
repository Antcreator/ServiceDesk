using ServiceDesk.Notifications.Service;
using ServiceDesk.Util;

var host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
    {
        services.AddSingleton<QueueService>();
        services.AddHostedService<QueueWorkerService>();
    })
    .Build();

await host.RunAsync();
