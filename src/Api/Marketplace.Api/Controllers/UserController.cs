using Marketplace.Api.Attributes;
using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries.IQuery;
using Marketplace.Domain.Constants;
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

    [AddRoles(Roles.Admin, Roles.User)]
    [HttpPut("update")]
    public async ValueTask<ActionResult> UpdateUser(UpdateUser updateUser)
        => (await _userUpdateCommand.UpdateUser(updateUser)).Match<ActionResult>(Ok, BadRequest);

    [AddRoles(Roles.Admin)]
    [HttpGet("users")]
    public ActionResult AllUsers() =>
        _userReadQuery.AllUsers()
            .Match<ActionResult>(Ok, UnprocessableEntity);
}