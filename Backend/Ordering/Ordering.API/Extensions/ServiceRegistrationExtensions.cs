using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus.Abstractions;
using Ordering.API.Controllers;
using Ordering.Application.Commands;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Filters;
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

        return new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithProperty("ApplicationContext", "Ordering")
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http/localhost:8086" : seqServerUrl)
            .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://localhost:8085" : logstashUrl)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}