using System.Net;
using System.Text.Json;
using Inventory.Domain.Exceptions;

namespace Inventory.Api.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, 
        ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleErrorAsync(context, exception);
        }
    }

    private static Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        var errorCode = "error";
        const HttpStatusCode statusCode = HttpStatusCode.BadRequest;
        errorCode = exception switch
        {
            AuthException e => e.Code,
            _ => errorCode
        };
        var response = new { code = errorCode, message = exception.Message };
        var payload = JsonSerializer.Serialize(response);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(payload);
    }
}