using System.Text.Json;
using Basket.Entities;
using Basket.Models;
using StackExchange.Redis;

namespace Basket.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;
    private readonly ILogger<BasketRepository> _logger;
    private readonly ConnectionMultiplexer _redis;

    public BasketRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
    {
        _logger = loggerFactory.CreateLogger<BasketRepository>();
        _redis = redis;
        _database = redis.GetDatabase();
    }

    public async Task<BasketDeleted> DeleteBasketAsync(string id)
    {
        await _database.KeyDeleteAsync(id);
        return new BasketDeleted(Guid.Parse(id), "deleted");
    }

    public IEnumerable<string> GetUsers()
    {
        var server = GetServer();
        var data = server.Keys();

        return data?.Select(k => k.ToString());
    }

    public async Task<ShoppingCart> GetBasketAsync(string customerId)
    {
        var data = await _database.StringGetAsync(customerId);

        if (data.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<ShoppingCart>(data, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
    {
        var created = await _database.StringSetAsync(basket.Username.ToString(), JsonSerializer.Serialize(basket));

        if (!created)
        {
            _logger.LogInformation("Problem occur persisting the item.");
            return null;
        }

        _logger.LogInformation("Basket item persisted succesfully.");

        return await GetBasketAsync(basket.Username.ToString());
    }

    private IServer GetServer()
    {
        var endpoint = _redis.GetEndPoints();
        return _redis.GetServer(endpoint.First());
    }
}