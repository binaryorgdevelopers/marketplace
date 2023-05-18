using MediatR;
using Ordering.Application.Extensions;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Application.Commands;

public class CreateOrderDraftCommandHandler : IRequestHandler<CreateOrderDraftCommand, OrderDraftDto>
{
    public Task<OrderDraftDto> Handle(CreateOrderDraftCommand request, CancellationToken cancellationToken)
    {
        var order = Order.NewDraft();
        var orderItems = request.Items.Select(i => i.ToOrderItemDto());

        foreach (var item in orderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl,
                item.Units);
        }

        return Task.FromResult(OrderDraftDto.FromOrder(order));
    }
}

public record OrderDraftDto
{
    public IEnumerable<CreateOrderCommand.OrderItemDTO> OrderItems { get; init; }
    public decimal Total { get; init; }

    public static OrderDraftDto FromOrder(Order order)
    {
        return new OrderDraftDto()
        {
            OrderItems = order.OrderItems.Select(oi => new CreateOrderCommand.OrderItemDTO
            {
                Discount = oi.GetCurrentDiscount(),
                ProductId = oi.ProductId,
                UnitPrice = oi.GetUnitPrice(),
                PictureUrl = oi.GetPictureUri(),
                Units = oi.GetUnits(),
                ProductName = oi.GetOrderItemProductName()
            }),
            Total = order.GetTotal()
        };
    }
}