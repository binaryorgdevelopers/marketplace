using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Domain.Events;

public class OrderStatusChangedToAwaitingValidationDomainEvent:INotification
{
    public OrderStatusChangedToAwaitingValidationDomainEvent(Guid orderId, IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }

    public Guid OrderId { get; }
    public IEnumerable<OrderItem> OrderItems { get; }
    
    
}