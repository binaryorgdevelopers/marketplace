using NotificationService.Hubs;
using NotificationService.Messages.Events;

namespace NotificationService.Services;

public class HubService : IHubService
{
    private readonly IHubWrapper _hubWrapper;

    public HubService(IHubWrapper hubWrapper)
    {
        _hubWrapper = hubWrapper;
    }

    public async Task PublishOperationPendingAsync(OperationPending @event)
        => await _hubWrapper.PublishToUserAsync(@event.UserId,
            "operation_pending",
            new
            {
                id = @event.Id,
                name = @event.Name,
                resource = @event.Resource
            });

    public async Task PublishOperationCompletedAsync(OperationCompleted @event)
        => await _hubWrapper.PublishToUserAsync(@event.UserId,
            "operation_completed",
            new
            {
                id = @event.Id,
                name = @event.Name,
                resource = @event.Resource
            }
        );

    public async Task PublishOperationRejectedAsync(OperationRejected @event)
        => await _hubWrapper.PublishToUserAsync(@event.UserId,
            "operation_rejected",
            new
            {
                id = @event.Id,
                name = @event.Name,
                resource = @event.Resource,
                code = @event.Code,
                reason = @event.Message
            }
        );
}