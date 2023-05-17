using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Domain.Events;

public class OrderCancelledDomainEvent:INotification
{
    public OrderCancelledDomainEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
    
}