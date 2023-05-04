using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EventBusRabbitMq;

public static class ConsumerConfiguration
{
    public static IServiceCollection RegisterRmqConsumer(this IServiceCollection services, RmqConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            // foreach (var consumer in config.Consumers)
            // {
            //     x.AddConsumer(consumer.Consumer.GetType());
            // }

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config.Host, "/", h =>
                {
                    h.Username(config.Username);
                    h.Password(config.Username);
                });

                foreach (var consumer in config.Consumers)
                {
                    cfg.ReceiveEndpoint(consumer.Queue, ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));

                        ep.ConfigureConsumer(context, consumer.Consumer.GetType());
                    });
                }
            });
        });
        return services;
    }
}

public record RmqConfiguration(string Host, string Username, string Password, params ConsumerConfig[] Consumers);

public record ConsumerConfig(IConsumer Consumer, string Queue);