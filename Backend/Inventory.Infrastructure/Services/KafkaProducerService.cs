using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Marketplace.Infrastructure.Services;

public class KafkaProducerService : IHostedService
{
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerService(IOptions<KafkaConfiguration> options, ILogger<KafkaProducerService> logger)
    {
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = options.Value.Host
        };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // for (int i = 0; i < 100; i++)
        // {
        //     var value = $"Hello world {i}";
        //     _logger.LogInformation(value);
        //     await _producer.ProduceAsync("notification", new Message<Null, string>()
        //     {
        //         Value = value
        //     }, cancellationToken);
        // }

        _producer.Flush(TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _producer.Dispose();
        return Task.CompletedTask;
    }
}