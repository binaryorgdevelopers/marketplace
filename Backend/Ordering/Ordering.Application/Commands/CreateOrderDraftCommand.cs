using MediatR;
using Ordering.Application.Models;

namespace Ordering.Application.Commands;

public class CreateOrderDraftCommand : IRequest<OrderDraftDto>
{
    public Guid BuyerId { get; private set; }

    public IEnumerable<BasketItem> Items { get; private set; }

    public CreateOrderDraftCommand(Guid buyerId, IEnumerable<BasketItem> items)
    {
        BuyerId = buyerId;
        Items = items;
    }
}