using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace WebHost;

public static class WebHostExtensions
{
    public static bool IsInKubernetes(this IWebHost webHost)
    {
        var cfg = webHost.Services.GetService<IConfiguration>();
        var orchestrationType = cfg.GetValue<string>("OrchestrationType");
        return orchestrationType?.ToUpper() == "K8S";
    }

    public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        var underK8s = webHost.IsInKubernetes();

        using var scope = webHost.Services.CreateScope();
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
                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(
                        retryCount: retries,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, timeSpan) =>
                        {
                            logger.LogWarning(exception,
                                "[{prefix}] Exception with message {Message} detected on attempt {attempt} of {retries}",
                                nameof(TContext), exception.GetType().Name, exception.Message, retries);
                        }
                    );

                retry.Execute(() => InvokeSeeder(seeder, context, services));
            }

            logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured while migrating the database used on context {DbContextName}",
                typeof(TContext).Name);
            if (underK8s) throw; //Rethrow under k8s because we rely on k8s to re-run the pod
        }

        return webHost;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
        IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}