using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Abstraction.Messaging;
using Shared.Messages;
using Shared.Models;
using Shared.Models.Constants;

namespace Marketplace.Application.Commands.Command.Authentication;

public class UserCreateCommandHandler : ICommandHandler<UserCreateCommand, AuthResult>
{
    private readonly IGenericRepository<User> _genericRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserCreateCommandHandler(IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator, IGenericRepository<User> genericRepository)
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _genericRepository = genericRepository;
    }

    public async Task<Result<AuthResult>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var searchResult = await _genericRepository.GetAsync(e => e.Email == request.Email);
        if (searchResult is not null)
        {
            return Result.Failure<AuthResult>(new Error(Codes.EmailInUse,
                $"With ${nameof(request.Email)}: '{request.Email}' already exists"));
        }

        User userTable = new User(Guid.NewGuid(), request.Email, request.Role, request.Firstname, request.Lastname,
            request.PhoneNumber);
        userTable.SetPassword(request.Password, _passwordHasher);
        await _genericRepository.AddAsync(userTable);
        var authResult =
            new AuthResult(
                new Authorized(
                    userTable.Id,
                    userTable.FirstName,
                    userTable.LastName,
                    userTable.PhoneNumber,
                    userTable.Email,
                    userTable.Role.ToString()),
                _jwtTokenGenerator.GenerateToken(new TokenRequest(userTable.Id, userTable.Email, userTable.PhoneNumber,
                    userTable.FirstName,
                    userTable.LastName, request.Role)));
        return Result.Success(authResult);
    }
}