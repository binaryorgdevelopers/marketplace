using Marketplace.Application.Commands.ICommand;
using Marketplace.Domain.Abstractions.Repositories;

namespace Marketplace.Api.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IJwtTokenGenerator tokenGenerator,
        IClientMergeCommand clientMergeCommand)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = tokenGenerator.ValidateJwtToken(token);
        if (userId != null)
        {
            context.Items["Client"] = clientMergeCommand.GetClientsById(userId);
        }

        await _next(context);
    }
}