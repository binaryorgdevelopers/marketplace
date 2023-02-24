using Marketplace.Application.Common;
using Marketplace.Application.Common.Extensions;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Application.Queries.IQuery.Auth;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Marketplace.Domain.Models.Constants;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Queries.Query.Auth;

public class CustomerReadQuery : ICustomerReadQuery
{
    private readonly IGenericRepository<Customer> _customer;
    private readonly IPasswordHasher<Customer> _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public CustomerReadQuery(IGenericRepository<Customer> customer, IPasswordHasher<Customer> passwordHasher,
        IJwtTokenGenerator tokenGenerator)
    {
        _customer = customer;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public Either<AuthResult, Exception> SignIn(SignInCommand signInCommand)
    {
        var user = _customer.Get(c => c.Email == signInCommand.Email);
        if (user is null)
            return new Either<AuthResult, Exception>(new AuthException(Codes.UserNotFound,
                $"user with email:'{signInCommand.Email}' not found"));
        var validationResult = user.ValidatePassword(signInCommand.Password, _passwordHasher);
        if (!validationResult)
            return new Either<AuthResult, Exception>(new AuthException(Codes.InvalidPassword, "Invalid password"));
        user.LastSession = DateTime.Now.SetKindUtc();
        _customer.Update(user);

        return new Either<AuthResult, Exception>(new AuthResult(
            new Authorized(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.Email, Roles.Customer),
            _tokenGenerator.GenerateToken(user.ToTokenRequest())));
    }
}