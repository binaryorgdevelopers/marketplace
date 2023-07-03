using System.Text;
using Authentication.Exceptions;
using Authentication.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;

namespace Authentication.Extensions;

public static class AuthExtensions
{
    public static void UseCustomMiddlewares(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<JwtMiddleware>();
        builder.UseMiddleware<ErrorHandlerMiddleware>();
    }

    public static WebApplicationBuilder AddCustomAuthentication(this WebApplicationBuilder builder)
    {
        var options = builder.Configuration.GetOptions<JwtOptions>(JwtOptions.SectionName);
        builder.Services.AddSingleton(options);
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
        builder.Services.AddAuthentication(opt =>
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret)),
                ValidIssuer = options.Issuer,
                ValidAudience = options.ValidAudience,
                ValidateAudience = options.ValidateAudience,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            jwtOptions.Events = new JwtBearerEvents
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
        return builder;
    }

    public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
    {
        var model = new TModel();
        configuration.GetSection(section).Bind(model);
        return model;
    }
}