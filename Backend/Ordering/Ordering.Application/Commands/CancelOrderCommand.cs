using System.Runtime.Serialization;
using MediatR;

namespace Ordering.Application.Commands;

public class CancelOrderCommand:IRequest<bool>
{
    [DataMember]
    public Guid OrderNumber { get; set; }

    public CancelOrderCommand()
    {
        
    }

    public CancelOrderCommand(Guid orderNumber)
    {
        OrderNumber = orderNumber;
    }
}