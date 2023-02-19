using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries.IQuery;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly IUserUpdateCommand _userUpdateCommand;
    private readonly IUserReadQuery _userReadQuery;

    public UserController(IUserUpdateCommand userUpdateCommand, IUserReadQuery userReadQuery)
    {
        _userUpdateCommand = userUpdateCommand;
        _userReadQuery = userReadQuery;
    }

    [HttpPut("update")]
    public async ValueTask<ActionResult> UpdateUser(UpdateUser updateUser)
    {
        var result = await _userUpdateCommand.UpdateUser(updateUser);
        return result.Match<ActionResult>(Ok, BadRequest);
    }

    [HttpGet("users")]
    public ActionResult AllUsers() =>
        _userReadQuery.AllUsers().Match<ActionResult>(Ok, UnprocessableEntity);
}