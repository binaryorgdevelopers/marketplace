using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Abstractions.Services;
using Marketplace.Domain.Entities;
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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddCloudStorage()
            .AddScoped<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddScoped<IPasswordHasher<Seller>, PasswordHasher<Seller>>()
            .AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>()
            .AddScoped<ILoggingBroker, LoggingBroker>()
            .AddHostedService<UserManagerService>();
        return services;
    }

    public static void AddDatabase(this IServiceCollection services, bool isDevEnv,
        IConfiguration configuration)
    {
  
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetValue<string>("Postgresql:ConnectionString"))
        );
    }

    private static IServiceCollection AddCloudStorage(this IServiceCollection services)
    {
        services.AddScoped<ICloudUploaderService, CloudUploaderService>();
        return services;
    }
}