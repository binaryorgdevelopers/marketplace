using System.Text.Json;
using Basket.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Events;

namespace Basket.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<ShoppingCart> GetBasket(Guid userName)
    {
        string? basket = await _redisCache.GetStringAsync(userName.ToString());
        return (string.IsNullOrEmpty(basket) ? null : JsonSerializer.Deserialize<ShoppingCart>(basket)) ??
               new ShoppingCart(userName);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
    {
        await _redisCache.SetStringAsync(basket.Username.ToString(), JsonSerializer.Serialize(basket));
        return await GetBasket(basket.Username);
    }

    public async Task<BasketDeleted> DeleteBasket(Guid userName)
    {
        await _redisCache.RemoveAsync(userName.ToString());
        return new BasketDeleted(userName, "200");
    }
}