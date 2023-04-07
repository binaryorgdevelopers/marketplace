using Basket.Entities;
using Shared.Events;

namespace Basket.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasketAsync(string customerId);
    IEnumerable<string> GetUsers();
    Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);
    Task<BasketDeleted> DeleteBasketAsync(string id);
}