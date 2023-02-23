using Marketplace.Application.Commands.ICommand.Authentication;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries;
using Marketplace.Application.Queries.IQuery;
using Marketplace.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserCreateCommand _userCreateCommand;
    private readonly IUserSignInQuery _userSignInQuery;
    
    private readonly ICustomerCreateCommand _customerCreateCommand;
    private readonly ICustomerReadQuery _customerReadQuery;

    private readonly ISellerCreateCommand _sellerCreateCommand;
    private readonly ISellerReadQuery _sellerReadQuery;

    public AuthController(IUserCreateCommand userCreateCommand, IUserSignInQuery userSignInQuery,
        ICustomerReadQuery customerReadQuery, ICustomerCreateCommand customerCreateCommand, ISellerReadQuery sellerReadQuery, ISellerCreateCommand sellerCreateCommand)
    {
        _userCreateCommand = userCreateCommand;
        _userSignInQuery = userSignInQuery;
        _customerReadQuery = customerReadQuery;
        _customerCreateCommand = customerCreateCommand;
        _sellerReadQuery = sellerReadQuery;
        _sellerCreateCommand = sellerCreateCommand;
    }

    [HttpPost("user/sign-up")]
    public async Task<ActionResult> Register(SignUp signUp) =>
        (await _userCreateCommand.CreateUser(signUp)).Match<ActionResult>(Ok, BadRequest);


    [HttpPost("user/sign-in")]
    public async Task<ActionResult>? SignIn(SignInCommand signInCommand) =>
        (await _userSignInQuery.SignIn(signInCommand)).Match<ActionResult>(Ok, BadRequest);


    [HttpPost("customer/sign-up")]
    public async Task<ActionResult> CustomerSignUp(CustomerCreate customerCreate) =>
        (await _customerCreateCommand.CreateCustomer(customerCreate)).Match<ActionResult>(Ok, BadRequest);

    [HttpPost("customer/sign-in")]
    public ActionResult CustomerSignIn(SignInCommand signInCommand) =>
        _customerReadQuery.SignIn(signInCommand).Match<ActionResult>(Ok, BadRequest);

    [HttpPost("seller/sign-up")]
    public async Task<ActionResult> SellerSignUp([FromForm]SellerCreate sellerCreate) =>
        (await _sellerCreateCommand.CreateSeller(sellerCreate)).Match<ActionResult>(Ok, BadRequest);

    [HttpPost("seller/sign-in")]
    public ActionResult SellerSignIn(SignInCommand signInCommand) =>
        _sellerReadQuery.SellerSignIn(signInCommand).Match<ActionResult>(Ok, BadRequest);
}