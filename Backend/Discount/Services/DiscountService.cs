using AutoMapper;
using Discount.gRPC.Protos;
using Discount.Repositories;
using Grpc.Core;

namespace Discount.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _discountRepository;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
    {
        _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _discountRepository.GetDiscount(request.ProductId);

        if (coupon == null)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Discount with ProductName={request.ProductId} not found"));
        _logger.LogInformation("Discount is successfully created. ProductName:{ProductName}", coupon.ProductName);

        var couponModel = new CouponModel
        {
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            Amount = coupon.Amount,
            ProductId= coupon.ProductId
        };
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        // var coupon = _mapper.Map<Coupon>(request.Coupon);
        var result = await _discountRepository.CreateDiscount(request.Coupon);
        _logger.LogInformation("Discount is successfully created. ProductName:{ProductName}",
            request.Coupon.ProductName);
        // var couponModel = new CouponModel
        // {
        //     ProductName = coupon.ProductName,
        //     Description = coupon.Description,
        //     Amount = coupon.Amount,
        //     Id = coupon.Id
        // };
        return request.Coupon;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        // var coupon = _mapper.Map<Coupon>(request.Coupon);
        await _discountRepository.UpdateDiscount(request.Coupon);
        _logger.LogInformation("Discount is successfully updated. ProductName:{ProductName}",
            request.Coupon.ProductName);
        // var couponModel = new CouponModel
        // {
        //     ProductName = coupon.ProductName,
        //     Description = coupon.Description,
        //     Amount = coupon.Amount,
        //     Id = coupon.Id
        // };
        Console.WriteLine("Calling");
        return request.Coupon;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var deleted = await _discountRepository.DeleteDiscount(request.ProductName);
        var response = new DeleteDiscountResponse
        {
            Success = deleted
        };
        return response;
    }
}