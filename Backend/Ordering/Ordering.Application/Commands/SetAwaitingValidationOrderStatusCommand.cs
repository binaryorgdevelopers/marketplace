using System.Runtime.Serialization;
using MediatR;

namespace Ordering.Application.Commands;

public class SetAwaitingValidationOrderStatusCommand : IRequest<bool>
{
    [DataMember] public Guid OrderNumber { get; private set; }

    public SetAwaitingValidationOrderStatusCommand(Guid orderNumber)
    {
        OrderNumber = orderNumber;
    }
}