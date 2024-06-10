using ServiceDesk.Notifications;
using ServiceDesk.Notifications.Service;
using ServiceDesk.Util;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddSingleton<QueueService>();
builder.Services.AddHostedService<QueueWorkerService>();

var app = builder.Build();

app.MapHub<NotificationHub>("/api/notify");

await app.RunAsync();
