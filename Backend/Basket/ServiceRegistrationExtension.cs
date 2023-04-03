using Basket.Repositories;
using Basket.Services;
using Discount.disc;
using StackExchange.Redis;

namespace Basket;

public static class ServiceRegistrationExtension
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBasketRepository, BasketRepository>();
        builder.Services.AddScoped<DiscountGrpcService>();
        return builder;
    }

    public static WebApplicationBuilder AddGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o =>
            o.Address = new Uri(builder.Configuration.GetValue<string>("Grpc:Host")!));
        return builder;
    }

    public static WebApplicationBuilder AddRedis(this WebApplicationBuilder builder)
    {
        string options = builder.Configuration.GetValue<string>("Redis:Host")!;
        string password = builder.Configuration.GetValue<string>("Redis:Password")!;
        builder.Services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = options;
            option.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { options },
                Password = password,
                AbortOnConnectFail = false
            };
        });

        return builder;
    }
}