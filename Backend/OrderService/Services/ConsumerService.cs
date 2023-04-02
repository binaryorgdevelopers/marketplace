using System.Text;
using Kafka.Public;
using Kafka.Public.Loggers;
using Microsoft.Extensions.Options;
using Shared.Models;


namespace OrderService.Services;

public class ConsumerService : IHostedService
{
    private readonly ILogger<ConsumerService> _logger;
    private readonly string topic = "test";
    private readonly string groupId = "test_group";
    private readonly string host;
    private readonly ClusterClient _cluster;

    public ConsumerService(IOptions<KafkaConfiguration> options, ILogger<ConsumerService> logger
    )
    {
        _logger = logger;
        host = options.Value.Host;
        _cluster = new ClusterClient(new Configuration()
        {
            Seeds = host
        }, new ConsoleLogger());
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cluster.ConsumeFromLatest(topic);
        _cluster.MessageReceived += record =>
        {
            _logger.LogInformation($"Received :{Encoding.UTF8.GetString(record.Value as byte[])}");
        };
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
       _cluster?.Dispose();
       return Task.CompletedTask;
    }
}