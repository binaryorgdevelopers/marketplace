using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using EventBus.Abstractions;
using EventBus.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.IntegrationEvents.Events;
using Serilog.Context;

namespace Ordering.Application.IntegrationEvents.EventHandling;

public class OrderStockRejectedIntegrationEventHandler : IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderStockRejectedIntegrationEventHandler> _logger;


    public OrderStockRejectedIntegrationEventHandler(IMediator mediator,
        ILogger<OrderStockRejectedIntegrationEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(OrderStockRejectedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id} - {Program.AppName}")) ;
        {
            _logger.LogInformation(
                "----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})",
                @event.Id,
                Program.AppName,
                @event
            );
            var orderStockRejectedItems = @event.OrderStockItems
                .FindAll(c => !c.HasStock)
                .Select(c => c.ProductId)
                .ToList();

            var command = new SetStockRejectedOrderStatusCommand(@event.OrderId, orderStockRejectedItems);

            _logger.LogInformation(
                "------ Sending command: {CommandName} - {Property}: {CommandId} ({@Command})",
                command.GetGenericTypeName(),
                nameof(command.OrderNumber),
                command.OrderNumber,
                command
            );
            await _mediator.Send(command);
        }
    }
}