using System.Runtime.Serialization;
using MediatR;

namespace Ordering.Application.Commands;

public class ShipOrderCommand : IRequest<bool>
{
    [DataMember] public Guid OrderNumber { get; private set; }

    public ShipOrderCommand(Guid orderNumber)
    {
        OrderNumber = orderNumber;
    }
}