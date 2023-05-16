using EventBus.Abstractions;
using EventBus.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents;
using Ordering.Domain.Abstractions;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Application.Commands;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;
    private readonly IOrderingIntegrationEventService _integrationEventService;

    public CancelOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus,
        IOrderingIntegrationEventService integrationEventService)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
        _integrationEventService = integrationEventService;
    }

    public async Task<bool> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
        if (orderToUpdate == null) return false;
        orderToUpdate.SetCancelledStatus();
        await _integrationEventService.AddAndSaveEventAsync(
            new IntegrationEvent(command.OrderNumber, DateTime.Now));
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

// Use for Idempotency in Command process
public class CancelOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CancelOrderCommand, bool>
{
    public CancelOrderIdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<CancelOrderCommand, bool>> logger)
        : base(mediator, requestManager, logger)
    {
    }

    protected override bool CreateResultForDuplicateRequests()
    {
        return true;
    }
}