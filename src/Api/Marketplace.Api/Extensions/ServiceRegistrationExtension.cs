using System.Text;
using FluentValidation;
using Marketplace.Api.Middleware;
using Marketplace.Api.Services;
using Marketplace.Api.Validations;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Constants;
using Marketplace.Infrastructure;
using Marketplace.Infrastructure.Common.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Role = Marketplace.Domain.Entities.Role;

namespace Marketplace.Api.Extensions;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddHostedService<InitService>();
        services.AddRedis(configuration);
        return services;
    }
    
    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<JwtMiddleware>();
        builder.UseMiddleware<ErrorHandlerMiddleware>();
        return builder;
    }

    public static IServiceCollection RegisterLambda(this IServiceCollection services)
    {
        services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
        return services;
    }

    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<JwtOptions>(JwtOptions.SectionName);
        services.AddSingleton(options);
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwtOptions =>
        {
            jwtOptions.SaveToken = true;
            jwtOptions.RequireHttpsMetadata = false;
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                ValidIssuer = options.Issuer,
                ValidAudience = options.ValidAudience,
                ValidateAudience = options.ValidateAudience,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            jwtOptions.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Authentication-Token-Expired", "true");
                    }

                    return Task.CompletedTask;
                }
            };
        });
        return services;
    }

    public static void AddRoleBasedAuth(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole(Role.Admin));
            options.AddPolicy("AdminUserPolicy", policy => policy.RequireRole(Role.Admin, Role.User));
        });
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(x => x.ConfigurationOptions = new ConfigurationOptions
        {
            EndPoints = { configuration.GetValue<string>("Redis:host")! },
            Password = configuration.GetValue<string>("Redis:password")!
        });
        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<SignIn>, SignInValidation>();
        services.AddScoped<IValidator<SignUp>, SignUpValidation>(); 
        services.AddScoped<IValidator<UpdateUser>, UpdateUserValidation>();
        return services; 
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme()
            {
                Description = ""
            });
        });
    }
}