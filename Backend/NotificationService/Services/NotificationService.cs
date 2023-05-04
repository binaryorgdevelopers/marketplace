using NotificationService.Dtos;
using NotificationService.Persistence.Entities;
using Shared.Abstraction.MediatR;
using Shared.Constants;

namespace NotificationService.Services;

public class NotificationService : INotificationService
{
    private readonly IRequestHandler<NotificationRequest, Notification> _addNotificationRequestHandler;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IRequestHandler<NotificationRequest, Notification> addNotificationRequestHandler,
        ILogger<NotificationService> logger)
    {
        _addNotificationRequestHandler = addNotificationRequestHandler;
        _logger = logger;
    }

    public Task SendNotificationAsync(string textMessage)
    {
        var cancellationToken = new CancellationTokenSource().Token;
        var notification = System.Text.Json.JsonSerializer.Deserialize<Notification>(textMessage);
        if (notification is not null)
        {
            switch (notification.Key)
            {
                case Keys.Discount:
                    _addNotificationRequestHandler.HandleAsync(new NotificationRequest(notification.UserId,
                        notification.Title, notification.Message, notification.MessageContent), cancellationToken);
                    break;
            }
        }

        _logger.LogInformation(textMessage);
        return Task.CompletedTask;
    }
}