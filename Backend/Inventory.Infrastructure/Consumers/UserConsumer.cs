using EventBus.Models;
using Inventory.Domain.Abstractions.Repositories;
using MassTransit;

namespace Marketplace.Infrastructure.Consumers;

public class UserConsumer : IConsumer<UserToken>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserConsumer()
    {
    }

    public UserConsumer(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task Consume(ConsumeContext<UserToken> context)
        => await Task.Run(() =>
        {
            // var message = context.Message;
            // var user = _jwtTokenGenerator.ValidateJwtToken(message.Token);
            //
            // context.RespondAsync(new UserDto(userId.Value));
            // return Task.FromResult(Task.CompletedTask);
        });
}