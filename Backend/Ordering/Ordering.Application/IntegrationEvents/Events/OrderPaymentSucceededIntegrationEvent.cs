using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderPaymentSucceededIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public OrderPaymentSucceededIntegrationEvent(int orderId) => OrderId = orderId;
}