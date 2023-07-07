using System.Text.Json;
using StackExchange.Redis;

namespace Identity.Infrastructure.Repositories;

public class CacheRepository : ICacheRepository
{
    private readonly IDatabase _database;
    private readonly ConnectionMultiplexer _redis;
    private readonly ILogger<CacheRepository> _logger;

    public CacheRepository(ConnectionMultiplexer redis,ILogger<CacheRepository> logger)
    {
        _database = redis.GetDatabase();
        _redis = redis;
        _logger = logger;
    }

    public async ValueTask<T?> SetStringAsync<T>(string key, T value) where T : class
    {
        var created = await _database.StringSetAsync($"user:{key}", JsonSerializer.Serialize(value));
        return !created ? null : await this.GetStringAsync<T>(key);
    }

    public async ValueTask<T?> GetStringAsync<T>(string key) where T : class
    {
        var data = await _database.StringGetAsync($"user:{key}");
        if (data.IsNullOrEmpty) return null;
        T deserialized = JsonSerializer.Deserialize<T>(data);
        return deserialized;
    }
}

public interface ICacheRepository
{
    ValueTask<T?> SetStringAsync<T>(string key, T value) where T : class;
    ValueTask<T?> GetStringAsync<T>(string key) where T : class;
}