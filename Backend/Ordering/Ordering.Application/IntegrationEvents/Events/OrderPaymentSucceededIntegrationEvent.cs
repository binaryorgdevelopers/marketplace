using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderPaymentSucceededIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public OrderPaymentSucceededIntegrationEvent(Guid orderId) => OrderId = orderId;
}