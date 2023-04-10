using Ordering.API.Application.Models;
using Ordering.Application.Commands;
using Ordering.Application.Models;

namespace Ordering.Application.Extensions;

using static CreateOrderCommand;

public static class BasketItemExtensions
{
    public static IEnumerable<OrderItemDTO> ToOrderItemsDto(this IEnumerable<BasketItem> basketItems)
    {
        foreach (var item in basketItems)
        {
            yield return item.ToOrderItemDto();
        }
    }

    public static OrderItemDTO ToOrderItemDto(this BasketItem item)
    {
        return new OrderItemDTO()
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            PictureUrl = item.PictureUrl,
            UnitPrice = item.UnitPrice,
            Units = item.Quantity
        };
    }
}
