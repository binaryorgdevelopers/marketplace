using Authentication;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Infrastructure.Services;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Shared.Models;
using StackExchange.Redis;

namespace Inventory.Api;

public static class ProgramExtensions
{
    public static WebApplicationBuilder RegisterLambda(this WebApplicationBuilder builder)
    {
        builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
        return builder;
    }

    public static WebApplicationBuilder RegisterMediatR(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Result).Assembly));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CustomerCreateCommand).Assembly));

        return builder;
    }

    public static WebApplicationBuilder AddCustomControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        return builder;
    }

    public static WebApplicationBuilder AddRedis(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Services.AddStackExchangeRedisCache(x => x.ConfigurationOptions = new ConfigurationOptions
        {
            EndPoints = { configuration.GetValue<string>("Redis:host")! },
            Password = configuration.GetValue<string>("Redis:password")!
        });
        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITokenValidator, ValidatorService>();
        return builder;
    }
}