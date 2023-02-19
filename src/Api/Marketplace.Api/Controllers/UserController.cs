using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Common.Messages.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly IUserUpdateCommand _userUpdateCommand;

    public UserController(IUserUpdateCommand userUpdateCommand)
    {
        _userUpdateCommand = userUpdateCommand;
    }

    [HttpPut("update")]
    public async ValueTask<ActionResult> UpdateUser(UpdateUser updateUser)
    {
        var result = await _userUpdateCommand.UpdateUser(updateUser);
        return result.Match<ActionResult>(Ok, BadRequest);
    }
}