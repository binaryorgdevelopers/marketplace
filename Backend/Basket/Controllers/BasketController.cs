using Basket.Entities;
using Basket.Repositories;
using Basket.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Controllers;

[ApiController]
[Route("basket")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
    {
        _basketRepository = basketRepository;
        _discountGrpcService = discountGrpcService;
    }


    [HttpGet("{username}", Name = "GetBasket")]
    public async Task<ActionResult<ShoppingCart>> GetBasket(Guid username)
        => Ok(await _basketRepository.GetBasket(username));


    [HttpDelete("{userName}", Name = "DeleteBasket")]
    public async Task<ActionResult> Delete(Guid userName)
        => Ok(await _basketRepository.DeleteBasket(userName));

    [HttpPost(Name = "UpdateBasket")]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket( ShoppingCart basket)
    {
        foreach (var item in basket.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }

        var result = await _basketRepository.UpdateBasket(basket);

        return Ok(result);
    }
}