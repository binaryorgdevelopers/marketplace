using EventBus.Abstractions;
using EventBus.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.IntegrationEvents.Events;
using Serilog.Context;

namespace Ordering.Application.IntegrationEvents.EventHandling;

public class
    UserCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> _logger;

    public UserCheckoutAcceptedIntegrationEventHandler(IMediator mediator,
        ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Integration event handler which starts the create order process
    /// </summary>
    /// <param name="event">
    /// Integration event message which is sent by the
    /// basket.api once it has successfully process the
    /// order items
    /// </param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Handle(UserCheckoutAcceptedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id} - {Program.AppName}"))
        {
            _logger.LogInformation(
                "------ Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})",
                @event.Id,
                Program.AppName,
                @event
            );

            var result = false;
            if (@event.RequestId == Guid.Empty)
            {
                using (LogContext.PushProperty("IdentifiedCommandId", @event.RequestId))
                {
                    var createOrderCommand = new CreateOrderCommand(
                        @event.Basket.Items, @event.UserId, @event.UserName,
                        @event.City, @event.Street, @event.State, @event.Country,
                        @event.ZipCode, @event.CardNumber, @event.CardHolderName,
                        @event.CardExpiration, @event.CardSecurityNumber, @event.CardTypeId);

                    var requestCreateOrder =
                        new IdentifiedCommand<CreateOrderCommand, bool>(@event.RequestId, createOrderCommand);

                    _logger.LogInformation(
                        "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        requestCreateOrder.GetGenericTypeName(),
                        nameof(requestCreateOrder.Id),
                        requestCreateOrder.Id,
                        requestCreateOrder
                    );
                    result = await _mediator.Send(requestCreateOrder);

                    if (result)
                    {
                        _logger.LogInformation("----- CreateOrderCommand succeeded - RequestId: {RequestId}",
                            @event.RequestId);
                    }
                    else
                    {
                        _logger.LogInformation("----- CreateOrderCommand failed - RequestId: {RequestId}",
                            @event.RequestId);
                    }
                }
            }
            else
            {
                _logger.LogInformation("Invalid IntegrationEvent -  RequestId is missing - {@IntegrationEvent}",
                    @event);
            }
        }
    }
}