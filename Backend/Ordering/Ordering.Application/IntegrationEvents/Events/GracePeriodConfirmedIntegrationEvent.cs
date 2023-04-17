using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record GracePeriodConfirmedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public GracePeriodConfirmedIntegrationEvent(int orderId) => OrderId = orderId;
}