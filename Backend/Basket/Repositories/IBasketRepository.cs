using Basket.Entities;
using Shared.Events;

namespace Basket.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(Guid userName);
    Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
    Task<BasketDeleted> DeleteBasket(Guid userName);
}