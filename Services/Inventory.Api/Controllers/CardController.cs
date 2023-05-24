using Authentication.Attributes;
using Authentication.Enum;
using Marketplace.Application.Common.Messages.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/card")]
public class CardController : ControllerBase
{
    private readonly ISender _sender;

    public CardController(ISender sender)
    {
        _sender = sender;
    }

    [AddRoles(Roles.Admin, Roles.Customer, Roles.User)]
    [HttpPost("bind")]
    public async ValueTask<IActionResult> BindCardToUser(BindCardToUserCommand cardToUserCommand)
    {
        var result = await _sender.Send(cardToUserCommand);
        return Ok(result);
    }

    [AddRoles(Roles.Admin, Roles.Customer, Roles.User)]
    [HttpGet("{guid}")]
    public async ValueTask<IActionResult> GetUserCards(Guid guid)
    {
        var result = await _sender.Send(new CardByUserIdCommand(guid));
        return Ok(result);
    }
}