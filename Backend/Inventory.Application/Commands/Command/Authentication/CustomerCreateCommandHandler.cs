using Authentication.Enum;
using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Events;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Inventory.Domain.Models.Constants;
using Inventory.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Commands.Command.Authentication;

internal sealed class CustomerCreateCommandHandler : ICommandHandler<CustomerCreateCommand, AuthResult>
{
    private readonly IGenericRepository<Customer> _customer;
    private readonly IPasswordHasher<Customer> _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public CustomerCreateCommandHandler(IGenericRepository<Customer> customer, IPasswordHasher<Customer> passwordHasher,
        IJwtTokenGenerator tokenGenerator)
    {
        _customer = customer;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<AuthResult>> Handle(CustomerCreateCommand customerCreateCommand,
        CancellationToken cancellationToken)
    {
        var existsAsync = await _customer.ExistsAsync(c => c.Email == customerCreateCommand.Email);
        if (existsAsync)
            return Result.Failure<AuthResult>(
                new Error("400", $"User with email :'{customerCreateCommand.Email}' is already exist"));

        var customer = new Customer(Guid.NewGuid(), customerCreateCommand.PhoneNumber, customerCreateCommand.Email,
            customerCreateCommand.FirstName, customerCreateCommand.LastName, customerCreateCommand.Username);

        customer.SetPassword(customerCreateCommand.Password, _passwordHasher);

        customer.Authorities = new[] { Roles.Customer.ToString(), Keys.PHONE_UNVERIFIED };
        await _customer.AddAsync(customer);

        return Result.Success(new AuthResult(
            new Authorized(customer.Id, customer.FirstName, customerCreateCommand.LastName, customer.PhoneNumber,
                customer.Email, Roles.Customer), _tokenGenerator.GenerateToken(customer.ToTokenRequest())));
    }
}