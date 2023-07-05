using Microsoft.AspNetCore.SignalR;
using Shared.Extensions;

namespace NotificationService.Hubs;

public class NotificationsHub : Hub, IHubWrapper
{
    private readonly IHubContext<NotificationsHub> _hubContext;

    public NotificationsHub(IHubContext<NotificationsHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("Initialized", $"Client with Id:' {Context.ConnectionId} ' connected");
    }

    public async Task PublishToUserAsync(Guid userId, string message, object data) =>
        await Clients.Group(userId.ToUserGroup()).SendAsync(message, data);

    public async Task PublishToAllAsync(string message, object data)
    {
        await _hubContext.Clients.All.SendAsync("all", new { message, data });
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}