using Marketplace.Application.Commands;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Interface.Authentication;
using Marketplace.Application.Common.Interface.Database;
using Marketplace.Application.Queries;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Repositories;
using Marketplace.Infrastructure.Common.Authentication;
using Marketplace.Infrastructure.Database;
using Marketplace.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<IUserCreateCommand, UserCreateCommand>();
        services.AddScoped<IUserSignInQuery, UserSignInQuery>();

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddDatabase();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase("devEnv"); });
        return services;
    }
}