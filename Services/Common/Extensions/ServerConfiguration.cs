using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;

public static class ServerConfiguration
{
    public static WebApplicationBuilder AddCustomGrpcServer<T>(this WebApplicationBuilder builder)
        where T : class
    {
        builder.Services.AddGrpc();
        builder.Services.AddScoped<T>();
        return builder;
    }

    public static WebApplicationBuilder AddCustomGrpcServer<TInterface, TImplementation>(this WebApplicationBuilder builder)
        where TImplementation : class, TInterface
        where TInterface : class
    {
        builder.Services.AddGrpc();
        builder.Services.AddSingleton<TInterface, TImplementation>();
        return builder;
    }

    public static WebApplicationBuilder AddCustomGrpcPorts(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(options =>
        {
            var ports = GetDefinedPorts(builder.Configuration);
            options.ListenLocalhost(ports.httpPort,
                listenOptions => listenOptions.Protocols = HttpProtocols.Http1AndHttp2);
            options.ListenLocalhost(ports.grpcPort, o => o.Protocols =
                HttpProtocols.Http2);
        });
        return builder;
    }

    private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration configuration)
    {
        var grpcPort = Convert.ToInt32(configuration.GetValue<string>("Grpc:PORT"));
        var port = Convert.ToInt32(configuration.GetValue<string>("Grpc:HttpPort"));
        return (port, grpcPort);
    }
}