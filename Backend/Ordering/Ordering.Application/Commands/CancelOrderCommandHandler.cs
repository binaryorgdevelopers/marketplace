using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Application.Commands;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
        if (orderToUpdate == null) return false;
        orderToUpdate.SetCancelledStatus();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}