using Authentication;
using Basket.Models;
using Basket.Repositories;
using Basket.Services;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Basket;

public static class BasketExtensions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBasketRepository, BasketRepository>();
        builder.Services.AddScoped<ITokenValidator, ValidatorService>();

        return builder;
    }

    public static WebApplicationBuilder AddRedis(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<BasketOptions>(builder.Configuration.GetSection("Redis"));
        builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<BasketOptions>>().Value;
            var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);
            configuration.Password = settings.Password;

            return ConnectionMultiplexer.Connect(configuration);
        });

        return builder;
    }
}