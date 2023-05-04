using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderStatusChangeToCancelledIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public string OrderStatus { get; }
    public string BuyerName { get; }

    public OrderStatusChangeToCancelledIntegrationEvent(Guid orderId, string orderStatus, string buyerName)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
    }
}