using Marketplace.Application.Commands;
using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserCreateCommand _userCreateCommand;
    private readonly IUserSignInQuery _userSignInQuery;

    public AuthController(IUserCreateCommand userCreateCommand, IUserSignInQuery userSignInQuery)
    {
        _userCreateCommand = userCreateCommand;
        _userSignInQuery = userSignInQuery;
    }

    [HttpPost("sign-up")]
    public async Task<ActionResult> Register(SignUp signUp)
    {
        var result = await _userCreateCommand.CreateUser(signUp);
        return result.Match<ActionResult>(Ok, BadRequest);
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult>? SignIn(SignIn signIn)
    {
        var result = await _userSignInQuery.SignIn(signIn);
        return result.Match<ActionResult>(Ok, BadRequest);
    }
}