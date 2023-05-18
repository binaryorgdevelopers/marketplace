using EventBus.Models;
using Microsoft.AspNetCore.Http;

namespace Authentication.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ITokenValidator validator)
    {
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Split("")
            .Last();
        if (token != null)
        {
            var user = await validator.ValidateToken(new UserToken(token!));
            if (user != null)
            {
                context.Items["User"] = user;
            }
        }


        await _next(context);
    }
}