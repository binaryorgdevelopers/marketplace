using Authentication;
using Basket.Models;
using Basket.Repositories;
using Basket.Services;
using Microsoft.Extensions.Options;
using Shared.Redis;
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
}