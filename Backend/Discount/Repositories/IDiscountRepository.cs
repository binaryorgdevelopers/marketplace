using Discount.gRPC.Protos;

namespace Discount.Repositories;

public interface IDiscountRepository
{
    Task<CouponModel> GetDiscount(Guid productId);
    Task<bool> CreateDiscount(CouponModel coupon);
    Task<bool> UpdateDiscount(CouponModel coupon);
    Task<bool> DeleteDiscount(Guid productId);
}