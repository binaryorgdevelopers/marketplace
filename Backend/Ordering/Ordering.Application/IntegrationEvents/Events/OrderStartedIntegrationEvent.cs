using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderStartedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; init; }

    public OrderStartedIntegrationEvent(Guid userId) => UserId = userId;
}