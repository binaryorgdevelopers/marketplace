using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Application.Commands;

public class SetPaidOrderStatusCommandHandler : IRequestHandler<SetPainOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetPaidOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Handler which processes the command when Shipment service confirms the payment
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> Handle(SetPainOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // Simulate a work time for validating the payment

        await Task.Delay(10000, cancellationToken);

        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
        if (orderToUpdate == null) return false;

        orderToUpdate.SetPaidStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}