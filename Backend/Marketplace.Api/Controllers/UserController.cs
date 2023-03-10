using Marketplace.Api.Attributes;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Models.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

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
}