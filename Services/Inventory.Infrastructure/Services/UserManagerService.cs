using Marketplace.Application.Common.Messages.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Marketplace.Infrastructure.Services;

public class UserManagerService : BackgroundService
{
    private readonly ILogger<UserManagerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Timer? _timer = null;

    public UserManagerService(ILogger<UserManagerService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        do
        {
            await sender.Send(new ClientMergeCommand(), stoppingToken);
            
            await Task.Delay(3600000, stoppingToken);
        } while (!stoppingToken.IsCancellationRequested);
    }

    // private void DoWork(object? state)
    // {
    //     var count = Interlocked.Increment(ref _executionCount);
    //
    //     _logger.LogInformation($"Background Service is working. Count :{count}");
    // }

    // public override Task StartAsync(CancellationToken cancellationToken)
    // {
    //     _logger.LogInformation("Hosted service starting");
    //     _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3600000));
    //     return base.StartAsync(cancellationToken);
    // }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _timer?.Dispose();


        base.Dispose();
    }
}