using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderStatusChangedToSubmittedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public string OrderStatus { get; }
    public string BuyerName { get; }
    
    public OrderStatusChangedToSubmittedIntegrationEvent(Guid orderId, string orderStatus, string buyerName)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
    }
    
    
}