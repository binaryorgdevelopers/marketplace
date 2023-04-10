using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure.Idempotency;

namespace Ordering.Application.Commands;

public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public ShipOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }


    /// <summary>
    /// Handler which processes the command when administrator executes ship order from app
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> Handle(ShipOrderCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);

        if (orderToUpdate == null) return false;

        orderToUpdate.SetShippedStatus();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class ShipOrderIdentifiedCommandHandler : IdentifiedCommandHandler<ShipOrderCommand, bool>
{
    public ShipOrderIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<ShipOrderCommand, bool>> logger) : base(mediator, requestManager, logger)
    {
    }

    protected override bool CreateResultForDuplicateRequests()
    {
        return true; //Ignore duplicate requests for processing order
    }
}