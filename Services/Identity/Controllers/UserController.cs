using Identity.Infrastructure.Services;
using Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using Shared.Models;

namespace Identity.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class UserController : ControllerBase
{
    private readonly UserManagerService _userManagerService;

    public UserController(UserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    [HttpPost("register")]
    public async ValueTask<ActionResult> Register(UserCreateCommand createCommand)
    {
        var result = await Result
            .Create(createCommand)
            .Bind(c => _userManagerService.Register(c));
        return Ok(result);
    }

    [HttpPost("login")]
    public async ValueTask<ActionResult> Login(UserSignInCommand signInCommand)
    {
        var result = await Result
            .Create(signInCommand)
            .Bind(c => _userManagerService.Login(c));
        return Ok(result);
    }

    [HttpPost("change-password")] 
    public async ValueTask<IActionResult> ChangePassword(ChangePasswordCommand changePasswordCommand)
        => await Result
            .Create(changePasswordCommand)
            .Bind(c => _userManagerService.ChangePassword(c))
            .Match(Ok, BadRequest);

    [HttpPost("block")]
    public async ValueTask<IActionResult> BlockUser(UserByEmail userByEmail)
        => await Result
            .Create(userByEmail)
            .Bind(c => _userManagerService.BlockUser(userByEmail))
            .Match(Ok, BadRequest);
}