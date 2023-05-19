using Identity.Infrastructure.Services;
using Identity.Models;
using Microsoft.AspNetCore.Mvc;
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
        return Ok(result.Value);
    }

    [HttpGet]
    public ActionResult Get() => Ok("vmdjsf");
}