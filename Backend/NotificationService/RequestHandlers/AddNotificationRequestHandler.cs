using NotificationService.DAL;
using NotificationService.Dtos;
using NotificationService.Persistence.Entities;
using Shared.Abstraction.MediatR;

namespace NotificationService.RequestHandlers;

public class AddNotificationRequestHandler : IRequestHandler<NotificationRequest, Notification>
{
    private readonly NotificationContext _notificationContext;

    public AddNotificationRequestHandler(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public async ValueTask<Notification> HandleAsync(NotificationRequest request, CancellationToken cancellationToken)
    {
        var notification = Notification.Create(request.UserId, request.Title, request.Message,
            request.MessageContent, DateTime.Now);

        await _notificationContext.Notifications.AddAsync(notification, cancellationToken);
        return notification;
    }
}