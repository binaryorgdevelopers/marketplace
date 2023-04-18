﻿using System.Reflection;
using EventBus.Models;
using IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure.EventBus.Producers;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection RegisterMassTransitServices(this IServiceCollection services)
    {
        // Determines Interface type
        var producer = typeof(IProducer<,>);
        // Selects all classes implemented IProducer<>
        var classes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(c => c.GetTypes())
            .Where(x => producer.IsAssignableFrom(x) && !x.IsInterface);
        // Adds classes to DI Container
        // foreach (var @class in classes)
        // {
        //     services.AddScoped(producer, @class);
        // }

        services.AddScoped(typeof(IProducer<UserToken, UserId>), typeof(UserTokenProducer));

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderingContext>(options =>
        {
            options.UseNpgsql(configuration.GetValue<string>("ConnectionString"),
                npgsqlOptionsAction: builder =>
                {
                    builder.MigrationsAssembly(typeof(OrderingContext).GetTypeInfo().Assembly.GetName().Name);
                    builder.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });
        });
        services.AddDbContext<IntegrationEventLogContext>(options =>
        {
            options.UseNpgsql(configuration.GetValue<string>("ConnectionString"),
                npgsqlOptionsAction: builder =>
                {
                    builder.MigrationsAssembly(typeof(OrderingContext).GetTypeInfo().Assembly.GetName().Name);
                    builder.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });
        });
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}