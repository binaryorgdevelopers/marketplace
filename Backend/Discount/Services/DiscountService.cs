using Discount.gRPC.Protos;
using Discount.Repositories;
using Grpc.Core;

namespace Discount.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _discountRepository;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger)
    {
        _discountRepository = discountRepository;
        _logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _discountRepository.GetDiscount(Guid.Parse(request.ProductId));
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Discount with ProductName={request.ProductId} not found"));
        _logger.LogInformation("Discount is successfully created. ProductName:{ProductName}", coupon.ProductName);
        return coupon;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var result = await _discountRepository.CreateDiscount(request.Coupon);
        _logger.LogInformation("Discount is successfully created. ProductName:{ProductName}",
            request.Coupon.ProductName);
        return request.Coupon;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        await _discountRepository.UpdateDiscount(request.Coupon);
        _logger.LogInformation("Discount is successfully updated. ProductName:{ProductName}",
            request.Coupon.ProductName);
        return request.Coupon;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var deleted = await _discountRepository.DeleteDiscount(Guid.Parse(request.ProductName));
        var response = new DeleteDiscountResponse
        {
            Success = deleted
        };
        return response;
    }
}