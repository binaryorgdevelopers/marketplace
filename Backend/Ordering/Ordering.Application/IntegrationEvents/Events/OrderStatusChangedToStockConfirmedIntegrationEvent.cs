using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderStatusChangedToStockConfirmedIntegrationEvent : IntegrationEvent
{
    public OrderStatusChangedToStockConfirmedIntegrationEvent(Guid orderId, string orderStatus, string buyerName)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
    }

    public Guid OrderId { get; }
    public string OrderStatus { get; }
    public string BuyerName { get; }
    
}