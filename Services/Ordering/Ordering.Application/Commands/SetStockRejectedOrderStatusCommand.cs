using System.Runtime.Serialization;
using MediatR;

namespace Ordering.Application.Commands;

public class SetStockRejectedOrderStatusCommand : IRequest<bool>
{
    [DataMember] public Guid OrderNumber { get; private set; }
    [DataMember] public List<Guid> OrderStockItems { get; private set; }

    public SetStockRejectedOrderStatusCommand(Guid orderNumber,List<Guid> orderStockItems)
    {
        OrderNumber = orderNumber;
        OrderStockItems = orderStockItems;
    }
}