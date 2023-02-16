﻿using System.Net;
using System.Text.Json;
using Marketplace.Domain.Exceptions;

namespace Marketplace.Api.Middleware;

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
        var statusCode = HttpStatusCode.BadRequest;
        var message = "There was an error.";
        switch(exception)
        {
            case AuthException e:
                errorCode = e.Code;
                message = e.Message;
                break;
        }
        var response = new { code = errorCode, message = exception.Message };
        var payload = JsonSerializer.Serialize(response);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(payload);
    }
}