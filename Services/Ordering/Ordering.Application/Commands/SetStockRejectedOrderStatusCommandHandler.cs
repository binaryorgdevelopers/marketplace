﻿using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Abstractions;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Application.Commands;

public class SetStockRejectedOrderStatusCommandHandler : IRequestHandler<SetStockRejectedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetStockRejectedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Handler which processes the command when StockService rejects the request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> Handle(SetStockRejectedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(10000, cancellationToken);
        var orderToUpdate = await _orderRepository.GetAsync(request.OrderNumber);
        if (orderToUpdate == null) return false;

        orderToUpdate.SetCancelledStatusWhenStockIsRejected(request.OrderStockItems);

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class
    SetStockRejectedOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetStockRejectedOrderStatusCommand,
        bool>
{
    public SetStockRejectedOrderStatusIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<SetStockRejectedOrderStatusCommand, bool>> logger) : base(mediator,
        requestManager, logger)
    {
    }

    protected override bool CreateResultForDuplicateRequests()
    {
        return true; //Ignores duplicate requests for processing order;
    }
}