using Marketplace.Application.Commands;
using Marketplace.Application.Common.Messages.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserCreateCommand _userCreateCommand;

    public AuthController(IUserCreateCommand userCreateCommand)
    {
        _userCreateCommand = userCreateCommand;
    }

    [HttpPost]
    public async Task<ActionResult> Register(UserCreate userCreate)
    {
        var result = await _userCreateCommand.CreateUser(userCreate);
        return result.Match<ActionResult>(Ok, BadRequest);
    }
}