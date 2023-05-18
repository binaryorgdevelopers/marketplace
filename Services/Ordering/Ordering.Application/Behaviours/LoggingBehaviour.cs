using EventBus.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("----Handling Command {CommandName} ({@Command})", request.GetGenericTypeName(),
            request);

        var response = await next();

        _logger.LogInformation("----- Command {CommandName} handled - response : {@Response}",
            request.GetGenericTypeName(), response);

        return response;
    }
}