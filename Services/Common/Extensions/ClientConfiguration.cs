using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;

public static class ClientConfiguration
{
    public static WebApplicationBuilder AddCustomGrpcClient<T>(this WebApplicationBuilder builder)
        where T : class
    {
        var url = new Uri(builder.Configuration.GetValue<string>("Grpc:Host")!);
        builder.Services.AddGrpcClient<T>(options => options.Address = url);
        return builder;
    }
}