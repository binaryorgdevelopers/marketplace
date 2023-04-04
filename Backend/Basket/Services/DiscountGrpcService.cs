using Discount.gRPC.Protos;

namespace Basket.Services;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        _discountProtoService = discountProtoService;
    }

    public async Task<CouponModel> GetDiscount(string productId)
    {
        var discountRequest = new GetDiscountRequest { ProductId = productId };
        var result =  _discountProtoService.GetDiscount(discountRequest);
        await Task.CompletedTask;
        return result;
    }
}