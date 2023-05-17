using Basket.Entities;
using Basket.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Controllers;

[ApiController]
[Route("basket")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;

    public BasketController(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    [HttpGet]
    public ActionResult Get() => Ok("BasketService running");


    [HttpGet("{username}", Name = "GetBasket")]
    public async Task<ActionResult<ShoppingCart>> GetBasket(Guid username)
        => Ok(await _basketRepository.GetBasketAsync(username.ToString()));


    [HttpDelete("{userName}", Name = "DeleteBasket")]
    public async Task<ActionResult> Delete(Guid userName)
        => Ok(await _basketRepository.DeleteBasketAsync(userName.ToString()));

    [HttpPost(Name = "UpdateBasket")]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart basket)
    {
        var result = await _basketRepository.UpdateBasketAsync(basket);

        return Ok(result);
    }
}