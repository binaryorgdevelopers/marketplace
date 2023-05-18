using EventBus.Models;
using MassTransit;
using Ordering.Application.IntegrationEvents;

namespace Ordering.Infrastructure.EventBus.Producers;

public class UserTokenProducer : IProducer<UserToken, UserDto>
{
    private readonly IOrderingIntegrationEventService _integrationEventService;
    private readonly IRequestClient<UserDto> _requestClient;

    public UserTokenProducer(IOrderingIntegrationEventService integrationEventService)
    {
        _integrationEventService = integrationEventService;
        // _requestClient = requestClient;
    }

    public async Task<UserDto> Handle(UserToken request)
    {
        var response = await _requestClient.GetResponse<UserDto>(request);
        return response.Message;
    }
}