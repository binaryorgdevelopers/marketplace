using Marketplace.Application.Commands.Command;
using Marketplace.Application.Commands.Command.Authentication;
using Marketplace.Application.Commands.Command.CategoryCommands;
using Marketplace.Application.Commands.Command.Product;
using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Commands.ICommand.Authentication;
using Marketplace.Application.Commands.ICommand.CategoryCommand;
using Marketplace.Application.Commands.ICommand.Product;
using Marketplace.Application.Queries;
using Marketplace.Application.Queries.IQuery;
using Marketplace.Application.Queries.IQuery.Auth;
using Marketplace.Application.Queries.IQuery.CategoryQueries;
using Marketplace.Application.Queries.Query;
using Marketplace.Application.Queries.Query.Auth;
using Marketplace.Application.Queries.Query.CategoryQueries;
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
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddCloudStorage();
        services.AddScoped<IUserCreateCommand, UserCreateCommand>();
        services.AddScoped<IUserSignInQuery, UserSignInQuery>();

        services.AddScoped<IUserUpdateCommand, UserUpdateCommand>();
        services.AddScoped<IUserReadQuery, UserReadQuery>();

        services.AddScoped<ICustomerCreateCommand, CustomerCreateCommand>();
        services.AddScoped<ICustomerReadQuery, CustomerReadQuery>();

        services.AddScoped<ISellerCreateCommand, SellerCreateCommand>();
        services.AddScoped<ISellerReadQuery, SellerReadQuery>();

        services.AddScoped<IProductCreateCommand, ProductCreateCommand>();

        services.AddScoped<ICategoryCreateCommand, CategoryCreateCommand>();
        services.AddScoped<ICategoryReadQuery, CategoryReadQuery>();

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IPasswordHasher<Seller>, PasswordHasher<Seller>>();
        services.AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>();

        services.AddScoped<IClientMergeCommand, ClientMergeCommand>();
        services.AddScoped<ILoggingBroker, LoggingBroker>();

        services.AddHostedService<UserManagerService>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }

    public static void AddDatabase(this IServiceCollection services, bool isDevEnv,
        IConfiguration configuration)
    {
        const string postgres = "Postgresql:ConnectionString";
        const string postgresDev = "PostgresqlDev:ConnectionString";
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetValue<string>(isDevEnv ? postgresDev : postgres))
        );
    }

    private static void AddCloudStorage(this IServiceCollection services)
    {
        services.AddScoped<ICloudUploaderService, CloudUploaderService>();
    }
}