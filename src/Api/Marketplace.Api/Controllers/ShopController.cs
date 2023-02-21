using Marketplace.Api.Attributes;
using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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

    [AddRoles(Roles.Admin)]
    [HttpPost("add")]
    public async Task<ActionResult> CreateShop(ShopCreate shopCreate)
    {
        var result = await _shopCreateCommand.CreateShop(shopCreate);
        return result.Match<ActionResult>(Ok, BadRequest);
    }
}