using System.Runtime.Serialization;
using MediatR;

namespace Ordering.Application.Commands;

public class SetPainOrderStatusCommand : IRequest<bool>
{
    [DataMember] public int OrderNumber { get; private set; }

    public SetPainOrderStatusCommand(int orderNumber)
    {
        OrderNumber = orderNumber;
    }
}