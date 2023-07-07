using System.Data.Common;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMq;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using NotificationService.Hubs;
using NotificationService.IntegrationEvents.EventHandling;
using NotificationService.IntegrationEvents.Events;
using NotificationService.Persistence;
using RabbitMQ.Client;

namespace NotificationService;

public static class Extensions
{
    public static WebApplicationBuilder AddSignalR(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IHubWrapper, NotificationsHub>();
        builder.Services.AddCors(setup =>
        {
            setup.AddDefaultPolicy(policy =>
                policy.SetIsOriginAllowed(_ => true)
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
        builder.Services.AddSignalR();
        return builder;
    }

    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<NotificationContext>(options =>
            options.UseNpgsql(builder.Configuration.GetValue<string>("Database:ConnectionString")));
        return builder;
    }

    public static void ConfigureEventBus(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        eventBus
            .Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent,
                OrderStatusChangedToAwaitingValidationEventHandler>();
    }

    public static WebApplicationBuilder AddCustomIntegrations(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(sp =>
            (DbConnection c) => new IntegrationEventLogService(c));
        builder.Services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();

            var factory = new ConnectionFactory
            {
                HostName = builder.Configuration["EventBusConnection"],
                DispatchConsumersAsync = true
            };
            if (!string.IsNullOrEmpty(builder.Configuration["EventBusUserName"]))
            {
                factory.UserName = builder.Configuration["EventBusUserName"];
            }

            if (!string.IsNullOrEmpty(builder.Configuration["EventBusPassword"]))
            {
                factory.Password = builder.Configuration["EventBusPassword"];
            }

            var retryCount = 5;
            if (!string.IsNullOrEmpty(builder.Configuration["EventBusRetryCount"]))
            {
                retryCount = int.Parse(builder.Configuration["EventBusRetryCount"]!);
            }

            if (!string.IsNullOrEmpty(builder.Configuration["EventBusPort"]))
            {
                factory.Port = int.Parse(builder.Configuration["EventBusPort"]!);
            }

            return new DefaultRabbitMqPersistentConnection(factory, logger, retryCount);
        });
        return builder;
    }

    public static WebApplicationBuilder AddEventBus(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IEventBus, global::EventBusRabbitMq.EventBusRabbitMq>(sp =>
        {
            var subscriptionClientName = builder.Configuration["SubscriptionClientName"];
            var rabbitMqPersistenceConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
            var logger = sp.GetRequiredService<ILogger<global::EventBusRabbitMq.EventBusRabbitMq>>();
            var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

            var retryCount = 5;
            if (string.IsNullOrEmpty(builder.Configuration["EventBusRetryCount"]))
            {
                retryCount = int.Parse(builder.Configuration["EventBusRetryCount"]);
            }

            return new global::EventBusRabbitMq.EventBusRabbitMq(rabbitMqPersistenceConnection, logger, sp,
                eventBusSubscriptionManager, subscriptionClientName, retryCount);
        });
        builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IHubWrapper, NotificationsHub>();
        // builder.Services.AddSingleton<Hub>()

        return builder;
    }
}