using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderStatusChangedToPaidIntegrationEvent : IntegrationEvent
{
 public Guid OrderId { get; }
 public string orderStatus { get; }
 public string BuyerName { get; }
 public IEnumerable<OrderStockItem> OrderStockItems { get; }
 public OrderStatusChangedToPaidIntegrationEvent(Guid orderId, string orderStatus, string buyerName, IEnumerable<OrderStockItem> orderStockItems)
 {
  OrderId = orderId;
  this.orderStatus = orderStatus;
  BuyerName = buyerName;
  OrderStockItems = orderStockItems;
 }
 
}