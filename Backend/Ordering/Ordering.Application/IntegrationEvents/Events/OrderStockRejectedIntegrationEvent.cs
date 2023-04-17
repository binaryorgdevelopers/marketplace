using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderStockRejectedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public List<ConfirmedOrderStockItem> OrderStockItems { get; }
}

public record ConfirmedOrderStockItem
{
    public int ProductId { get; }
    public bool HasStock { get; }

    public ConfirmedOrderStockItem(bool hasStock, int productId)
    {
        HasStock = hasStock;
        ProductId = productId;
    }
}