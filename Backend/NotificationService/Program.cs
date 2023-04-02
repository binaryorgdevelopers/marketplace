using NotificationService;
using NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddKafka()
    .AddCommands()
    .AddSignalR()
    .AddDatabase()
    .AddServices();

var app = builder.Build();
app.UseCors();
app.MapHub<NotificationsHub>("/marketplace");
app.MapGet("/", () => "Notification service");

app.Run();