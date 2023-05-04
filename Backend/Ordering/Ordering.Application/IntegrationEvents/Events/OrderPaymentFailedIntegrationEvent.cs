using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderPaymentFailedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public OrderPaymentFailedIntegrationEvent(Guid orderId) => OrderId = orderId;
}