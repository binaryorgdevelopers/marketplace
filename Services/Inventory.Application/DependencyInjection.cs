using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(assembly));

        // services.AddValidatorsFromAssembly(assembly).AddMapster();

        return services;
    }
    //
    // private static void AddMapster(this IServiceCollection services)
    // {
    //     var typeAdapterConfigs = TypeAdapterConfig.GlobalSettings;
    //     Assembly applicationAssembly = typeof(BaseDto<,>).Assembly;
    //     typeAdapterConfigs.Scan(applicationAssembly);
    // }
}