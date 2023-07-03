using Authentication.Enum;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Inventory.Domain.Extensions;
using Marketplace.Application.Common.Messages.Commands;
using Microsoft.AspNetCore.Identity;
using Shared.Abstraction.Messaging;
using Shared.Messages;
using Shared.Models;
using Shared.Models.Constants;

namespace Marketplace.Application.Queries.Query.Auth;

public class CustomerSignInQueryHandler : ICommandHandler<CustomerSignInCommand, AuthResult>
{
    private readonly IGenericRepository<Customer> _customer;
    private readonly IPasswordHasher<Customer> _passwordHasher;
    
    private readonly IJwtTokenGenerator _tokenGenerator;

    public CustomerSignInQueryHandler(IGenericRepository<Customer> customer, IPasswordHasher<Customer> passwordHasher,
        IJwtTokenGenerator tokenGenerator)
    {
        _customer = customer;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }
    

    public async ValueTask<Result<AuthResult>> HandleAsync(CustomerSignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _customer.GetAsync(c => c.Email == request.Email);
        if (user is null)
            return Result.Failure<AuthResult>(new Error(Codes.UserNotFound,
                $"user with email:'{request.Email}' not found"));
        if (!user.Active)
            return Result.Failure<AuthResult>(new Error(Codes.InvalidCredential,
                "User is blocked, Please contact customer support"));
        var validationResult = user.ValidatePassword(request.Password, _passwordHasher);
        if (!validationResult)
            return Result.Failure<AuthResult>(new Error(Codes.InvalidPassword, "Invalid password"));
        user.LastSession = DateTime.Now.SetKindUtc();
        _customer.Update(user);

        return Result.Success(new AuthResult(
            new Authorized(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.Email, Roles.Customer.ToString()),
            _tokenGenerator.GenerateToken(user.ToTokenRequest())));
    }
}