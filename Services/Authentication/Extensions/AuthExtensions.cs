using Authentication.Exceptions;
using Authentication.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Authentication.Extensions;

public static class AuthExtensions
{
    public static void UseCustomMiddlewares(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<JwtMiddleware>();
        builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}