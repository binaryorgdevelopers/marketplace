using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;

public static class ServerConfiguration
{
    public static IServiceCollection AddCustomGrpcServer<T>(this IServiceCollection services)
        where T : class
    {
        services.AddGrpc();
        services.AddScoped<T>();
        return services;
    }

    public static IServiceCollection AddCustomGrpcServer<TInterface, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TInterface
        where TInterface : class
    {
        services.AddGrpc();
        services.AddSingleton<TInterface, TImplementation>();
        return services;
    }
}