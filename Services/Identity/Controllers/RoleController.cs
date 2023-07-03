using Authentication.Attributes;
using Authentication.Enum;
using Identity.Infrastructure.Services;
using Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("api/v1/role")]
public class RoleController : ControllerBase
{
    private readonly RoleManagerService _roleManagerService;

    public RoleController(RoleManagerService roleManagerService)
    {
        _roleManagerService = roleManagerService;
    }

    [HttpPost("create")]
    public async ValueTask<ActionResult> CreateRole(RoleCreateCommand createCommand)
    {
        var result = await _roleManagerService.CreateRole(createCommand);
        return Ok(result);
    }

    [HttpGet]
    [AddRoles(Roles.Admin)]
    public ActionResult Ok() => Ok("WORKING");
}