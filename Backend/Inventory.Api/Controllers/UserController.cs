using System.Reflection.Metadata.Ecma335;
using Authentication.Attributes;
using Authentication.Enum;
using Inventory.Api.Extensions;
using Inventory.Domain.Shared;
using Marketplace.Application.Common.Messages.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [AddRoles(Roles.Admin, Roles.User)]
    [HttpPut("update")]
    public async ValueTask<ActionResult> UpdateUser(UpdateUserCommand updateUserCommand)
    {
        var result = await _sender.Send(updateUserCommand);
        return Ok(result);
    }

    [AddRoles(Roles.Admin)]
    [HttpPost("block")]
    public async ValueTask<ActionResult> BlockUser(UserBlockCommand command)
    {
        var result = await _sender.Send(command);
        return Ok(result);
    }

    [AddRoles(Roles.Admin, Roles.Customer, Roles.User)]
    public async ValueTask<IActionResult> BindCardToUser(BindCardToUserCommand cardToUserCommand)
        =>
            await Result
                .Create(cardToUserCommand)
                .Bind(c => _sender.Send(c))
                .Match(Ok, BadRequest);
}