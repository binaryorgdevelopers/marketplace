using EventBus.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Abstractions;

namespace Ordering.Application.Commands;

/// <summary>
/// Provides a base implementation for handling duplicate request and ensuring idempotent updates, in the case where
/// a requestId sent by clients is used to detect duplicate requests.
/// </summary>
/// <typeparam name="T">Type of the command handler that performs the operation if request is not duplicated</typeparam>
/// <typeparam name="TR">Return value of the inner command handler</typeparam>
public class IdentifiedCommandHandler<T, TR> : IRequestHandler<IdentifiedCommand<T, TR>, TR> where T : IRequest<TR>
{
    private readonly IMediator _mediator;
    private readonly IRequestManager _requestManager;
    private readonly ILogger<IdentifiedCommandHandler<T, TR>> _logger;


    protected virtual TR CreateResultForDuplicateRequests()
    {
        return default;
    }

    public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<T, TR>> logger)
    {
        _mediator = mediator;
        _requestManager = requestManager;
        _logger = logger;
    }

    public async Task<TR> Handle(IdentifiedCommand<T, TR> request, CancellationToken cancellationToken)
    {
        var alreadyExists = await _requestManager.ExistsAsync(request.Id);

        if (alreadyExists)
        {
            return CreateResultForDuplicateRequests();
        }
        else
        {
            await _requestManager.CreateRequestCommandAsync<T>(request.Id);
            try
            {
                var command = request.Command;
                var commandName = command.GetGenericTypeName();
                var idProperty = Guid.Empty;
                var commandId = Guid.Empty;

                switch (command)
                {
                    case CreateOrderCommand createOrderCommand:
                        idProperty = createOrderCommand.UserId;
                        commandId = createOrderCommand.UserId;
                        break;
                    case CancelOrderCommand cancelOrderCommand:
                        idProperty =cancelOrderCommand.OrderNumber;
                        commandId = cancelOrderCommand.OrderNumber;
                        break;

                    case ShipOrderCommand shipOrderCommand:
                        idProperty = shipOrderCommand.OrderNumber;
                        commandId = shipOrderCommand.OrderNumber;
                        break;

                    default:
                        idProperty = Guid.Empty;
                        commandId =Guid.Empty;
                        break;
                }

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    commandName,
                    idProperty,
                    commandId,
                    command);

                // Send the embeded business command to mediator so it runs its related CommandHandler 
                var result = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation(
                    "----- Command result: {@Result} - {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    result,
                    commandName,
                    idProperty,
                    commandId,
                    command);

                return result;
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}