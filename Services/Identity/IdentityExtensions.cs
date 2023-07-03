using Authentication;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Identity;

public static class IdentityExtensions
{
    public static WebApplicationBuilder AddCustomDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
        builder.Services.AddDbContext<IdentityContext>(options => options.UseNpgsql(connectionString));
        return builder;
    }

    public static WebApplicationBuilder AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserManagerService>();
        builder.Services.AddScoped<RoleManagerService>();
        builder.Services.AddScoped<ITokenValidator, TokenValidator>();
        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        return builder;
    }

    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        var jwtOptions = builder.Configuration.GetSection("JwtSettings");
        builder.Services.Configure<JwtOptions>(jwtOptions);
        return builder;
    }
}