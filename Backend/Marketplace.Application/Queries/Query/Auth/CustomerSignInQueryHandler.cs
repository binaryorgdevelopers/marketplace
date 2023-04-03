using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Events;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Extensions;
using Marketplace.Domain.Models.Constants;
using Marketplace.Domain.Shared;
using Microsoft.AspNetCore.Identity;

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

    public async Task<Result<AuthResult>> Handle(CustomerSignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _customer.GetAsync(c => c.Email == request.Email);
        if (user is null)
            return Result.Failure<AuthResult>(new Error(Codes.UserNotFound,
                $"user with email:'{request.Email}' not found"));
        var validationResult = user.ValidatePassword(request.Password, _passwordHasher);
        if (!validationResult)
            return Result.Failure<AuthResult>(new Error(Codes.InvalidPassword, "Invalid password"));
        user.LastSession = DateTime.Now.SetKindUtc();
        _customer.Update(user);

        return Result.Success(new AuthResult(
            new Authorized(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.Email, Roles.Customer),
            _tokenGenerator.GenerateToken(user.ToTokenRequest())));
    }
}