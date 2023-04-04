using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Marketplace.Infrastructure.Repositories;

public class DistributedCacheRepository<TEntity> where TEntity : class, new()
{
    private readonly IDistributedCache _cache;
    private readonly IDatabase _db;

    public DistributedCacheRepository(IDistributedCache cache, IConfiguration configuration)
    {
        _cache = cache;
        _db = ConnectionMultiplexer.Connect(configuration.GetValue<string>("Redis:host")!).GetDatabase();
    }

    public async Task<TEntity?> GetAsync(string key)
    {
        var result = await _cache.GetAsync(key);
        return JsonSerializer.Deserialize<TEntity>(Encoding.UTF8.GetString(result!));
    }

    public async Task SetAsync(string key, TEntity entity)
    {
        await _cache.SetAsync(key, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(entity)));
    }

    public async Task<TEntity?> GetSetAsync(string key, TEntity entity)
    {
        var result = await GetAsync(key);
        if (result is not null)
        {
            await SetAsync(key, entity);
        }

        return await GetAsync(key);
    }
}