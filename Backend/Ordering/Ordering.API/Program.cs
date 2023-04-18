using System.Net;
using IntegrationEventLogEF;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Ordering.API.Extensions;
using Ordering.Infrastructure;
using Serilog;
using WebHost;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = ServiceRegistrationExtensions.CreateSerilogLogger(builder.Configuration);

builder.AddAutofacModules();
builder
    .AddCustomMvc()
    .RegisterMediatR()
    .AddCustomIntegrations()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .RegisterMassTransitServices()
    .AddDatabase(builder.Configuration)
    .AddControllers();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ServiceRegistrationExtensions.ConfigureEventBus(app);


try
{
    Log.Information("Configuring web host ({ApplicationContext})...", "Ordering");
    var host = BuildWebHost(builder.Configuration, args);
    Log.Information("Applying migrations ({ApplicationContext})...", "Ordering");

    host.MigrateDbContext<OrderingContext>((context, services) =>
    {
        var env = services.GetService<IWebHostEnvironment>();
        var settings = services.GetService<IOptions<OrderingSettings>>();
        var logger = services.GetService<ILogger<OrderingContextSeed>>();

        new OrderingContextSeed()
            .SeedAsync(context, env, settings, logger)
            .Wait();
    }).MigrateDbContext<IntegrationEventLogContext>((_, __) => { });
    Log.Information("Starting web host ({ApplicationContext})...", "Ordering");
    host.Run();
    return 0;
}
catch (Exception e)
{
    Log.Fatal("Program terminated unexpectedly ({ApplicationContext})", "Ordering");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}


IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
    Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
        .CaptureStartupErrors(false)
        .ConfigureKestrel(options =>
        {
            var ports = GetDefinedPorts(configuration);
            options.Listen(IPAddress.Any, ports.httpPort,
                listenOptions => { listenOptions.Protocols = HttpProtocols.Http1AndHttp2; });
            options.Listen(IPAddress.Any, ports.grpcPort,
                listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
        })
        .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseSerilog()
        .Build();

(int httpPort, int grpcPort) GetDefinedPorts(IConfiguration configuration)
{
    var grpcPort = configuration.GetValue("GRPC_PORT", 5001);
    var port = configuration.GetValue("PORT", 80);
    return (port, grpcPort);
}

public partial class Program
{
    public static string Namespace = typeof(Program).Namespace;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}