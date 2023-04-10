using MediatR;
using Ordering.API.Application.Models;
using Ordering.Application.Models;

namespace Ordering.Application.Commands;

public class CreateOrderDraftCommand : IRequest<OrderDraftDto>
{
    public string BuyerId { get; private set; }

    public IEnumerable<BasketItem> Items { get; private set; }

    public CreateOrderDraftCommand(string buyerId, IEnumerable<BasketItem> items)
    {
        BuyerId = buyerId;
        Items = items;
    }
}