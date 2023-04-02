using Microsoft.AspNetCore.SignalR;
using NotificationService.DAL.Entities;
using NotificationService.Dtos;
using Shared.Abstraction.MediatR;
using Shared.Extensions;

namespace NotificationService.Hubs;

public class NotificationsHub : Microsoft.AspNetCore.SignalR.Hub, IHubWrapper
{

    private readonly IRequestHandler<NotificationRequest, Notification> _notificationRequestHandler;


    public NotificationsHub(IRequestHandler<NotificationRequest, Notification> notificationRequestHandler)
    {
        _notificationRequestHandler = notificationRequestHandler;
    }


    private async Task ConnectAsync() => await Clients.Client(Context.ConnectionId).SendAsync("connected");
    private async Task DisconnectAsync() => await Clients.Client(Context.ConnectionId).SendAsync("disconnected");

    public async Task PublishToUserAsync(Guid userId, string message, object data) =>
        await Clients.Group(userId.ToUserGroup()).SendAsync(message, data);

    public async Task PublishToAllAsync(string message, object data) => await Clients.All.SendAsync(message, data);
}