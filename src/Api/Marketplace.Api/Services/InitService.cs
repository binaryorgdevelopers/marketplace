using Microsoft.Extensions.Caching.Distributed;

namespace Marketplace.Api.Services;

public class InitService:IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public InitService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();

        var cachePipe = new List<Task>()
        {
            cache.RemoveAsync("",token: cancellationToken)
        };
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}