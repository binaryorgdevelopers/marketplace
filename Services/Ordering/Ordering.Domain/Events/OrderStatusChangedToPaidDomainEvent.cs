using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Domain.Events;

/// <summary>
/// Event used when the order is paid
/// </summary>
public class OrderStatusChangedToPaidDomainEvent : INotification
{
    public Guid OrderId { get; }
    public IEnumerable<OrderItem> OrderItems { get; }

    public OrderStatusChangedToPaidDomainEvent(Guid orderId, IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }
}