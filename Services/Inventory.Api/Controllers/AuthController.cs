using Inventory.Api.Extensions;
using Marketplace.Application.Common.Messages.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("user/sign-up")]
    public async Task<IActionResult> Register(UserCreateCommand request, CancellationToken cancellationToken) =>
        await Result
            .Create(request)
            .Bind(command => _sender.Send(command, cancellationToken))
            .Match(Ok, BadRequest);

    [HttpPost("user/sign-in")]
    public async Task<IActionResult> SignIn(UserSignInCommand request) =>
        await Result
            .Create(request)
            .Bind(command => _sender.Send(command))
            .Match(Ok, BadRequest);

    [HttpPost("customer/sign-up")]
    public async Task<IActionResult> CustomerSignUp(CustomerCreateCommand request) =>
        await Result
            .Create(request)
            .Bind(command => _sender.Send(command))
            .Match(Ok, BadRequest);

    [HttpPost("customer/sign-in")]
    public async Task<IActionResult> CustomerSignIn(CustomerSignInCommand request) =>
        await Result
            .Create(request)
            .Bind(command => _sender.Send(command))
            .Match(Ok, BadRequest);


    [HttpPost("seller/sign-up")]
    public async Task<IActionResult> SellerSignUp([FromForm] SellerCreate request) =>
        await Result
            .Create(request)
            .Bind(command => _sender.Send(command))
            .Match(Ok, BadRequest);

    [HttpPost("seller/sign-in")]
    public async Task<IActionResult> SellerSignIn(SellerSignInCommand request) =>
        await Result
            .Create(request)
            .Bind(command => _sender.Send(command))
            .Match(Ok, BadRequest);
}