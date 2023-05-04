using System.Runtime.Serialization;
using MediatR;

namespace Ordering.Application.Commands;

public class SetPaidOrderStatusCommand : IRequest<bool>
{
    [DataMember] public Guid OrderNumber { get; private set; }

    public SetPaidOrderStatusCommand(Guid orderNumber)
    {
        OrderNumber = orderNumber;
    }
}