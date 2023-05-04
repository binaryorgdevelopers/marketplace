using System.Runtime.Serialization;
using MediatR;

namespace Ordering.Application.Commands;

public class SetStockConfirmedOrderStatusCommand : IRequest<bool>
{
    [DataMember] public Guid OrderNumber { get; private set; }

    public SetStockConfirmedOrderStatusCommand(Guid orderNumber) => OrderNumber = orderNumber;
}