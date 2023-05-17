using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Abstractions;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Application.Commands;

public class SetStockConfirmedOrderStatusCommandHandler : IRequestHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetStockConfirmedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Handler which processes the command when Stock service confirms the request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> Handle(SetStockConfirmedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(10000, cancellationToken);

        var orderToUpdate = await _orderRepository.GetAsync(request.OrderNumber);
        if (orderToUpdate == null) return false;

        orderToUpdate.SetStockConfirmedStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

//Use for idempotency in Command process
public class
    SetStockConfirmedStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand,
        bool>
{
    public SetStockConfirmedStatusIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, bool>> logger) : base(mediator,
        requestManager, logger)
    {
    }

    protected override bool CreateResultForDuplicateRequests()
    {
        return true; //Ignore duplicate requests for processing order.
    }
}