using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Inventory.Api;

namespace Ordering.API.Extensions;

public static class CatalogGrpcExtensions
{
    public static BuyerByIdRequest ToBuyerByIdRequest(this string id) => new()
    {
        BuyerId = id
    };

    public static ProductByIdRequest ToProductByIdRequest(this string id) => new()
    {
        ProductId = id
    };

    public static Address ToAddress(this BillingAddress billingAddress)
        => new(billingAddress.StreetAddress, billingAddress.City, billingAddress.State,
            billingAddress.Country, billingAddress.ZipCode);
}