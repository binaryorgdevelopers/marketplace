using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingValidationIntegrationEvent : IntegrationEvent
{
    public OrderStatusChangedToAwaitingValidationIntegrationEvent(Guid orderId, string orderStatus, string buyerName, IEnumerable<OrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
        OrderStockItems = orderStockItems;
    }

    public Guid OrderId { get; }
    public string OrderStatus { get; }
    public string BuyerName { get; }
    public IEnumerable<OrderStockItem> OrderStockItems { get; }
}

public record OrderStockItem
{
    public OrderStockItem(int units, Guid productId)
    {
        Units = units;
        ProductId = productId;
    }

    public Guid ProductId { get; }
    public int Units { get; }
}