using Marketplace.Application.Commands.Command;
using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Queries;
using Marketplace.Application.Queries.IQuery;
using Marketplace.Application.Queries.Query;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Database;
using Marketplace.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserCreateCommand, UserCreateCommand>();
        services.AddScoped<IUserSignInQuery, UserSignInQuery>();
        services.AddScoped<IUserUpdateCommand, UserUpdateCommand>();
        services.AddScoped<IShopCreateCommand, ShopCreateCommand>();
        services.AddScoped<IUserReadQuery, UserReadQuery>();

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, bool isDevEnv,
        IConfiguration configuration)
    {
        const string postgres = "PostgresqlDev:ConnectionString";
        const string postgresDev = "PostgresqlDev:ConnectionString";
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetValue<string>(isDevEnv ? postgresDev : postgres))
        );

        return services;
    }
}