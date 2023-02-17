using Marketplace.Application.Commands.Command;
using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Queries;
using Marketplace.Application.Queries.Query;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Repositories;
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
        ConfigurationManager configuration, bool IsDevEnv)
    {
        services.AddScoped<IUserCreateCommand, UserCreateCommand>();
        services.AddScoped<IUserSignInQuery, UserSignInQuery>();

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddDatabase(IsDevEnv, configuration);

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, bool isDevEnv,
        IConfiguration configuration)
    {
        if (isDevEnv)
        {
            services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase("devEnv"); });
        }
        else
        {
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(configuration.GetValue<string>("Postgresql:ConnectionString"))
            );
        }

        return services;
    }
}