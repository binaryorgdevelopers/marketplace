using Authentication;
using Basket.Repositories;
using Basket.Services;

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