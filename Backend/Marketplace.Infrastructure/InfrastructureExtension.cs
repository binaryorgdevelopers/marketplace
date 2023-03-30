using Amazon.S3;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Abstractions.Services;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Models;
using Marketplace.Infrastructure.Database;
using Marketplace.Infrastructure.Repositories;
using Marketplace.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddCloudStorage(configuration)
            .AddScoped<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddScoped<IPasswordHasher<Seller>, PasswordHasher<Seller>>()
            .AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>()
            .AddScoped<ILoggingBroker, LoggingBroker>()
            .AddScoped(typeof(SearchService<>))
            .AddHostedService<UserManagerService>();
        return services;
    }

    public static void AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetValue<string>("Postgresql:ConnectionString"));
            options.EnableSensitiveDataLogging();
        });
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