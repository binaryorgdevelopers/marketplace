using EventBus.Models;
using MassTransit;

namespace Ordering.Infrastructure.EventBus.Producers;

public class UserTokenProducer : IProducer<UserToken, UserId>
{
    private readonly IRequestClient<UserId> _requestClient;

    public UserTokenProducer(IRequestClient<UserId> requestClient)
    {
        _requestClient = requestClient;
    }

    public async Task<UserId> Handle(UserToken request)
    {
        var response = await _requestClient.GetResponse<UserId>(request);
        return response.Message;
    }
}