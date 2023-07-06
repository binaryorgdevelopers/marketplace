using Amazon.S3;
using Inventory.Domain.Abstractions;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Abstractions.Services;
using Inventory.Domain.Entities;
using Marketplace.Infrastructure.Persistence;
using Marketplace.Infrastructure.Repositories;
using Marketplace.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Models;

namespace Marketplace.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddCloudStorage(configuration);
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
}