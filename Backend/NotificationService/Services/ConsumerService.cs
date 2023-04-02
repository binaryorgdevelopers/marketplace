using System.Text;
using Kafka.Public;
using Kafka.Public.Loggers;
using Microsoft.Extensions.Options;
using Shared.Constants;
using Shared.Models;

namespace NotificationService.Services;

public class ConsumerService : IHostedService
{
    private readonly ILogger<ConsumerService> _logger;
    private readonly KafkaConfiguration _kafkaConfiguration = new();
    private readonly ClusterClient _cluster;
    private readonly INotificationService _notificationService;

    public ConsumerService(IHost host)
    {
        using var scope = host.Services.CreateScope();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<ConsumerService>>();
        _kafkaConfiguration.Host = scope.ServiceProvider.GetRequiredService<IOptions<KafkaConfiguration>>().Value.Host;
        _notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        _cluster = new ClusterClient(new Configuration
        {
            Seeds = _kafkaConfiguration.Host
        }, new ConsoleLogger());
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Starting...");
        _cluster.ConsumeFromLatest(Topics.Notification);
        _cluster.MessageReceived += record =>
        {
            string message = Encoding.UTF8.GetString(record.Value as byte[] ?? Array.Empty<byte>());
            _logger.LogInformation($"Request received :{message}");
            try
            {
                _notificationService.SendNotificationAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        };
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cluster?.Dispose();
        return Task.CompletedTask;
    }
}