// using Marketplace.Ordering.Ordering.Api;

namespace Ordering.Application.Models;

public class BasketItem
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string ProductName { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal OldUnitPrice { get; init; }
    public int Quantity { get; init; }
    public string PictureUrl { get; init; }

    // public BasketItem FromGrpRequest(OrderDraft orderDraft) => new BasketItem()
    // {
    //
    // };
}

