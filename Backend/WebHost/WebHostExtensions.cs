using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebHost;

public static class WebHostExtensions
{
    public static bool IsInKubernetes(this IServiceProvider service)
    {
        var cfg = service.GetService<IConfiguration>();
        var orchestrationType = cfg.GetValue<string>("OrchestrationType");
        return orchestrationType?.ToUpper() == "K8S";
    }

    public static IServiceProvider MigrateDbContext<TContext>(this IServiceProvider service,
        Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        var underK8s = service.IsInKubernetes();

        using var scope = service.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();
        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}",
                typeof(TContext).Name);

            if (underK8s)
            {
                InvokeSeeder(seeder, context, services);
            }
            else
            {
                int retries = 10;
                // var retry = Policy.Handle<SqlException>()
                //     .WaitAndRetry(
                //         retryCount: retries,
                //         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                //         onRetry: (exception, timeSpan) =>
                //         {
                //             logger.LogWarning(exception,
                //                 "[{prefix}] Exception with message {Message} detected on attempt {attempt} of {retries}",
                //                 nameof(TContext), exception.GetType().Name, exception.Message, retries);
                //         }
                //     );

                // retry.Execute(() =>
                InvokeSeeder(seeder, context, services);
                // );
            }

            logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while migrating the database used on context {typeof(TContext).Name}");
            if (underK8s) throw; //Rethrow under k8s because we rely on k8s to re-run the pod
        }

        return service;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
        IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}