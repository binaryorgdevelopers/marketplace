using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderPaymentFailedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public OrderPaymentFailedIntegrationEvent(int orderId) => OrderId = orderId;
}