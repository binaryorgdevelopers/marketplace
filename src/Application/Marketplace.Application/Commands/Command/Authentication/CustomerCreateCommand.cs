using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Commands.ICommand.Authentication;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Commands.Command.Authentication;

public class CustomerCreateCommand : ICustomerCreateCommand
{
    private readonly IGenericRepository<Customer> _customer;
    private readonly IPasswordHasher<Customer> _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public CustomerCreateCommand(IGenericRepository<Customer> customer, IPasswordHasher<Customer> passwordHasher,
        IJwtTokenGenerator tokenGenerator)
    {
        _customer = customer;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Either<AuthResult, Exception>> CreateCustomer(CustomerCreate customerCreate)
    {
        var existsAsync = await _customer.ExistsAsync(c => c.Email == customerCreate.Email);
        if (existsAsync)
            throw new AuthException(Codes.InvalidCredential,
                $"User with email :'{customerCreate.Email}' is already exist");

        Customer customer = new Customer(Guid.NewGuid(), customerCreate.PhoneNumber, customerCreate.Email,
            customerCreate.FirstName, customerCreate.LastName, customerCreate.Username);
        customer.SetPassword(customerCreate.Password, _passwordHasher);
        customer.Authorities = new[] { Roles.Customer.ToString(), Keys.PHONE_UNVERIFIED };
        await _customer.AddAsync(customer);
        return new Either<AuthResult, Exception>(
            new AuthResult(
                new Authorized(customer.Id, customer.FirstName, customerCreate.LastName, customer.PhoneNumber,
                    customer.Email, Roles.Customer), _tokenGenerator.GenerateToken(customer.ToTokenRequest())));
    }
}