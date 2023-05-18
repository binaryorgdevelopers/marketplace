using NotificationService;
using NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddSignalR()
    .AddDatabase()
    .AddCustomIntegrations()
    .AddEventBus();

var app = builder.Build();
app.UseCors();
app.MapHub<NotificationsHub>("/marketplace");
app.MapGet("/", () => "Notification service");
app.ConfigureEventBus();
app.Run();

namespace NotificationService
{
    public partial class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;

        public static readonly string AppName =
            Namespace[(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1)..];
    }
}