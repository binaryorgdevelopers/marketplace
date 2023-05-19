using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;

public static class ClientConfiguration
{
    public static IServiceCollection AddCustomGrpcClient<T>(this IServiceCollection services,
        IConfiguration configuration)
        where T : class
    {
        var url = new Uri(configuration.GetValue<string>("Grpc:Host")!);
        services.AddGrpcClient<T>(options => options.Address = url);
        return services;
    }
}