﻿using System.ComponentModel.DataAnnotations;
using System.Text;
using Marketplace.Api.Middleware;
using Marketplace.Api.Services;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Interface.Authentication;
using Marketplace.Infrastructure;
using Marketplace.Infrastructure.Common.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace Marketplace.Api.Extensions;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddHostedService<InitService>();
        services.AddRedis(configuration);
        return services;
    }

    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        => builder.UseMiddleware<ErrorHandlerMiddleware>();

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
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(cfg =>
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                    ValidIssuer = options.Issuer,
                    ValidAudience = options.ValidAudience,
                    ValidateAudience = options.ValidateAudience,
                    ValidateLifetime = options.ValidateLifetime,
                    ClockSkew = TimeSpan.Zero
                }
            );
        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(x => x.ConfigurationOptions = new ConfigurationOptions
        {
            EndPoints = { configuration.GetValue<string>("Redis:host")! },
            Password = configuration.GetValue<string>("Redis:password")!
        });
        return services;
    }
}