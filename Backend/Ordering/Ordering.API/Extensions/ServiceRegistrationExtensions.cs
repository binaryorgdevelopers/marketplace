using System.Data.Common;
using Authentication;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMq;
using IntegrationEventLogEF.Services;
using Ordering.API.Controllers;
using Ordering.API.Services;
using Ordering.Application;
using Ordering.Application.Commands;
using Ordering.Application.IntegrationEvents;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.Queries;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Filters;
using Ordering.Infrastructure.Services;
using RabbitMQ.Client;
using Serilog;

namespace Ordering.API.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceProvider AddAutofacModules(this WebApplicationBuilder builder)
    {
        var container = new ContainerBuilder();
        var assembly = typeof(CancelOrderCommandHandler).Assembly;
        builder.Services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(assembly));

        container.Populate(builder.Services);
        container.RegisterModule(new MediatorModule());
        container.RegisterModule(new ApplicationModules(builder.Configuration["ConnectionString"]));
        return new AutofacServiceProvider(container.Build());
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ITokenValidator, ValidatorService>();
        return builder;
    }

    public static WebApplicationBuilder AddCustomMvc(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options => { options.Filters.Add(typeof(HttpGlobalExceptionFilter)); })
            .AddApplicationPart(typeof(OrdersController).Assembly)
            .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });
        return builder;
    }

    public static void ConfigureEventBus(IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus
            .Subscribe<UserCheckoutAcceptedIntegrationEvent,
                IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>>();
        eventBus
            .Subscribe<GracePeriodConfirmedIntegrationEvent,
                IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>>();
        eventBus
            .Subscribe<OrderStockConfirmedIntegrationEvent,
                IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>>();
        eventBus
            .Subscribe<OrderStockRejectedIntegrationEvent,
                IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>>();
        eventBus
            .Subscribe<OrderPaymentFailedIntegrationEvent,
                IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>>();
        eventBus
            .Subscribe<OrderPaymentSucceededIntegrationEvent,
                IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>>();
    }

    public static void GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var config = builder.Build();

        if (config.GetValue("UseVault", false))
        {
            // Stay tuned
        }
    }

    public static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
    {
        var seqServerUrl = configuration["Serilog:SeqServerUrl"];
        var logstashUrl = configuration["Serilog:LogstashUrl"];

        var result = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithProperty("ApplicationContext", "Ordering")
            .Enrich.FromLogContext()
            .WriteTo.Console()
            // .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http/localhost:8086" : seqServerUrl)
            // .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://localhost:8085" : logstashUrl)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        return result;
    }

    public static WebApplicationBuilder RegisterMediatR(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(CancelOrderCommand).Assembly));

        return builder;
    }

    public static WebApplicationBuilder AddCustomIntegrations(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(sp =>
            (DbConnection c) => new IntegrationEventLogService(c));
        builder.Services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

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
                retryCount = int.Parse(builder.Configuration["EventBusRetryCount"]);
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
            var rabbitmqPersistenceConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
            var logger = sp.GetRequiredService<ILogger<global::EventBusRabbitMq.EventBusRabbitMq>>();
            var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

            var retryCount = 5;
            if (string.IsNullOrEmpty(builder.Configuration["EventBusRetryCount"]))
            {
                retryCount = int.Parse(builder.Configuration["EventBusRetryCount"]);
            }

            return new global::EventBusRabbitMq.EventBusRabbitMq(rabbitmqPersistenceConnection, logger, sp,
                eventBusSubscriptionManager, subscriptionClientName, retryCount);
        });
        builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        return builder;
    }

    public static WebApplicationBuilder AddQueries(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<StateService>();
        string connectionString = builder.Configuration.GetValue<string>("ConnectionString")!;
        builder.Services.AddScoped<IOrderQueries, OrderQueries>(_ =>
            new OrderQueries(connectionString, new StateService()));
        return builder;
    }
}