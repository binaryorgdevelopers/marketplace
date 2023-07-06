using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Shared.Redis;

public static partial class Extensions
{
    public static WebApplicationBuilder AddRedis(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
        builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
            var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);
            configuration.Password = settings.Password;
            
            return ConnectionMultiplexer.Connect(configuration);
        });
        return builder;
    }
}