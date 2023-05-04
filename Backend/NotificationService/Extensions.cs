using Microsoft.EntityFrameworkCore;
using NotificationService.DAL;
using NotificationService.Dtos;
using NotificationService.Hubs;
using NotificationService.Persistence.Entities;
using NotificationService.RequestHandlers;
using NotificationService.Services;
using Shared.Abstraction.MediatR;
using Shared.Models;

namespace NotificationService;

public static class Extensions
{
    public static WebApplicationBuilder AddKafka(this WebApplicationBuilder builder)
    {
        var options = builder.Configuration.GetSection("KafkaConfiguration");
        builder.Services.Configure<KafkaConfiguration>(options);
        builder.Services.AddHostedService<ConsumerService>();
        return builder;
    }

    public static WebApplicationBuilder AddCommands(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IRequestHandler<NotificationRequest, Notification>, AddNotificationRequestHandler>();

        return builder;
    }

    public static WebApplicationBuilder AddSignalR(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IHubWrapper, NotificationsHub>();
        builder.Services.AddScoped<IHubService, HubService>();
        builder.Services.AddCors(setup =>
        {
            setup.AddDefaultPolicy(policy =>
                policy.SetIsOriginAllowed(_ => true).AllowCredentials().AllowAnyHeader().AllowAnyMethod());
        });
        builder.Services.AddSignalR();
        return builder;
    }

    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<NotificationContext>(options =>
            options.UseNpgsql(builder.Configuration.GetValue<string>("Database:ConnectionString")));
        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<INotificationService, Services.NotificationService>();
        return builder;
    }
}