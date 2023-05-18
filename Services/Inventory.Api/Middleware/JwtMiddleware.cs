using Inventory.Domain.Abstractions.Repositories;
using Marketplace.Application.Common.Messages.Commands;
using MediatR;

namespace Inventory.Api.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IJwtTokenGenerator tokenGenerator, ISender sender)
    {
        if (context.Request.Protocol != "HTTP/2")
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = tokenGenerator.ValidateJwtToken(token);
            var client = (await sender.Send(new ClientByIdQuery(userId))).Value;
            if (userId != null)
            {
                context.Items["User"] = client;
            }
        }


        await _next(context);
    }
}