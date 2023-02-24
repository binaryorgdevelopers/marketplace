using System.Text;
using FluentValidation;
using Marketplace.Api.Middleware;
using Marketplace.Api.Validations;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Infrastructure;
using Marketplace.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Role = Marketplace.Domain.Entities.Role;

namespace Marketplace.Api.Extensions;

public static class Extension
{
    public static void AddServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddRedis(configuration);
    }

    public static void UseCustomMiddlewares(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<JwtMiddleware>();
        builder.UseMiddleware<ErrorHandlerMiddleware>();
    }

    public static void RegisterLambda(this IServiceCollection services)
    {
        services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
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

    private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(x => x.ConfigurationOptions = new ConfigurationOptions
        {
            EndPoints = { configuration.GetValue<string>("Redis:host")! },
            Password = configuration.GetValue<string>("Redis:password")!
        });
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<SignInCommand>, SignInValidation>();
        services.AddScoped<IValidator<SignUp>, SignUpValidation>();
        services.AddScoped<IValidator<UpdateUser>, UpdateUserValidation>();
        return services;
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Marketplace.API", Version = "V1" });


            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}