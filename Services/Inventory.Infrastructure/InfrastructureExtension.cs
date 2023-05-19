using Amazon.S3;
using EventBus.Models;
using Inventory.Domain.Abstractions;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Abstractions.Services;
using Inventory.Domain.Entities;
using Marketplace.Infrastructure.Consumers;
using Marketplace.Infrastructure.Persistence;
using Marketplace.Infrastructure.Repositories;
using Marketplace.Infrastructure.Services;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Models;

namespace Marketplace.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddCloudStorage(configuration)
            .AddKafka(configuration)
            .AddScoped<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddScoped<IPasswordHasher<Seller>, PasswordHasher<Seller>>()
            .AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>()
            .AddScoped<ILoggingBroker, LoggingBroker>()
            .AddScoped(typeof(SearchService<>))
            .AddHostedService<UserManagerService>()
            .AddRabbitMq(configuration);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetValue<string>("Postgresql:ConnectionString"));
            options.EnableSensitiveDataLogging();
        });
        return services;
    }

    private static IServiceCollection AddCloudStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var credentials = configuration.GetSection("AWSConfiguration");
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.Configure<AWSCredentials>(credentials);
        services.AddScoped<AWSCredentials>();
        services.AddSingleton<IAmazonS3, AmazonS3Client>();
        services.AddScoped<ICloudUploaderService, CloudUploaderService>();
        return services;
    }

    private static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("KafkaConfiguration");
        services.Configure<KafkaConfiguration>(options);
        services.AddHostedService<KafkaProducerService>();
        return services;
    }

    private static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
        services.AddMassTransit(mt =>
        {
            mt.AddConsumer<UserConsumer>();

            mt.UsingRabbitMq((cntxt, cfg) =>
            {
                cfg.Host(new Uri(settings.Uri), "/", c =>
                {
                    c.Username(settings.Username);
                    c.Password(settings.Password);
                });

                cfg.ReceiveEndpoint("user", (c) => c.Consumer<UserConsumer>());
            });
        });
        return services;
    }
}

public class KafkaConfiguration
{
    public string Host { get; set; }
}