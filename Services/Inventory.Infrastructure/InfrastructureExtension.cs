using System.Diagnostics;
using Amazon.S3;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Abstractions.Services;
using Marketplace.Infrastructure.Persistence;
using Marketplace.Infrastructure.Repositories;
using Marketplace.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Models;

namespace Marketplace.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddScoped<IProductRepository, ProductRepository>();

        services.AddOptions<FileServerConfiguration>()
            .BindConfiguration("FileServer")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        if (Convert.ToBoolean(configuration["UseCloud"]))
        {
            services.AddCloudStorage(configuration);
        }
        else
        {
            services.AddFileServer(configuration);
        }

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetValue<string>("Postgresql:ConnectionString"));
            options.EnableSensitiveDataLogging();
        });
        return services;
    }

    private static IServiceCollection AddCloudStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var credentials = configuration.GetSection("AWSConfiguration");
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.Configure<AWSCredentials>(credentials);
        services.AddScoped<AWSCredentials>();
        services.AddSingleton<IAmazonS3, AmazonS3Client>();
        services.AddScoped<ICloudUploaderService, CloudUploaderService>();
        return services;
    }

    private static IServiceCollection AddFileServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("fileServer", (serviceProvider, httpClient) =>
        {
            var serverSettings = serviceProvider.GetRequiredService<IOptions<FileServerConfiguration>>().Value;
            httpClient.DefaultRequestHeaders.Add("Username", serverSettings.Username);
            httpClient.BaseAddress = new Uri(serverSettings.Url);
        });
        services.AddScoped<ICloudUploaderService, FileUploaderService>();
        return services;
    }
}