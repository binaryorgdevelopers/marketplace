using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Common.Messages.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/v1/shop")]
public class ShopController : ControllerBase
{
    private readonly IShopCreateCommand _shopCreateCommand;

    public ShopController(IShopCreateCommand shopCreateCommand)
    {
        _shopCreateCommand = shopCreateCommand;
    }

    [HttpPost("add")]
    public async Task<ActionResult> CreateShop(ShopCreate shopCreate)
    {
        var result = await _shopCreateCommand.CreateShop(shopCreate);
        return result.Match<ActionResult>(Ok, BadRequest);
    }
}