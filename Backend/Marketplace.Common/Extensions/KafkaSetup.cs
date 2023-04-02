using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Models;

namespace Shared.Extensions;

public static class KafkaSetup
{
    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("KafkaConfiguration");
        services.Configure<KafkaConfiguration>(options);
        return services;
    }
}