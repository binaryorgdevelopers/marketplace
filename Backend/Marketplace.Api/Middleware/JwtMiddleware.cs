using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Abstractions.Repositories;
using MediatR;

namespace Marketplace.Api.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IJwtTokenGenerator tokenGenerator, ISender sender)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = tokenGenerator.ValidateJwtToken(token);
        var client = (await sender.Send(new ClientByIdQuery(userId))).Value;
        if (userId != null)
        {
            context.Items["Client"] = client;
        }

        await _next(context);
    }
}