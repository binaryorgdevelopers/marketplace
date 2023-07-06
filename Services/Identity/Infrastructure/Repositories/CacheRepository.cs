using System.Text.Json;
using StackExchange.Redis;

namespace Identity.Infrastructure.Repositories;

public class CacheRepository : ICacheRepository
{
    private readonly IDatabase _database;
    private readonly ConnectionMultiplexer _redis;

    public CacheRepository(IDatabase database, ConnectionMultiplexer redis)
    {
        _database = database;
        _redis = redis;
    }

    public async ValueTask<T?> SetStringAsync<T>(string key, T value) where T : class
    {
        var created = await _database.StringSetAsync(key, JsonSerializer.Serialize(value));
        return !created ? null : await this.GetStringAsync<T>(key);
    }

    public async ValueTask<T?> GetStringAsync<T>(string key) where T : class
    {
        var data = await _database.StringGetAsync(key);
        return data.IsNullOrEmpty
            ? null
            : JsonSerializer.Deserialize<T>(data!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}

public interface ICacheRepository
{
    ValueTask<T?> SetStringAsync<T>(string key, T value) where T : class;
    ValueTask<T?> GetStringAsync<T>(string key) where T : class;
}