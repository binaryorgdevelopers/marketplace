using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderStockRejectedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public List<ConfirmedOrderStockItem> OrderStockItems { get; }
}

public record ConfirmedOrderStockItem
{
    public Guid ProductId { get; }
    public bool HasStock { get; }

    public ConfirmedOrderStockItem(bool hasStock, Guid productId)
    {
        HasStock = hasStock;
        ProductId = productId;
    }
}