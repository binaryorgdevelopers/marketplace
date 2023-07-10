using Authentication.Attributes;
using Authentication.Enum;
using Identity.Infrastructure.Services;
using Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using Shared.Models;

namespace Identity.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class CardController : ControllerBase
{
    private readonly UserManagerService _userManagerService;

    public CardController(UserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    [AddRoles(Roles.User, Roles.Admin)]
    [HttpPost("card/bind")]
    public async Task<IActionResult> BindCardToUser(BindCardToUserCommand command)
        => await Result.Create(command)
            .Bind(c => _userManagerService.BindCardToUser(c))
            .Match(Ok, BadRequest);

    [AddRoles(Roles.User, Roles.Admin)]
    [HttpGet("card/{id:guid}")]
    public async Task<IActionResult> GetUserCards(Guid id)
        => await Result.Create(new UserById(id))
            .Bind(c => _userManagerService.CardByUserId(c))
            .Match(Ok, BadRequest);
}