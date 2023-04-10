using System.Runtime.Serialization;
using MediatR;

namespace Ordering.Application.Commands;

public class CancelOrderCommand:IRequest<bool>
{
    [DataMember]
    public int OrderNumber { get; set; }

    public CancelOrderCommand()
    {
        
    }

    public CancelOrderCommand(int orderNumber)
    {
        OrderNumber = orderNumber;
    }
}