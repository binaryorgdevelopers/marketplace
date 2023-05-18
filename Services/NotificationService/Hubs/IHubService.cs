using NotificationService.Messages.Events;

namespace NotificationService.Hubs;

public interface IHubService
{
    Task PublishOperationPendingAsync(OperationPending @event);
    Task PublishOperationCompletedAsync(OperationCompleted @event);
    Task PublishOperationRejectedAsync(OperationRejected @event);
}