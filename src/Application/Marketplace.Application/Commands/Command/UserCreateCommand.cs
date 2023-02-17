using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Marketplace.Domain.Models;
using Marketplace.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Commands.Command;

public class UserCreateCommand : IUserCreateCommand
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IGenericRepository<User> _genericRepository;

    public UserCreateCommand(IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator, IGenericRepository<User> genericRepository)
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _genericRepository = genericRepository;
    }

    public async Task<Either<AuthResult, AuthException>> CreateUser(SignUp user)
    {
        var searchResult = await _genericRepository.GetAsync(c => c.Email == user.Email);
        if (searchResult is not null)
        {
            return new Either<AuthResult, AuthException>(new AuthException(Codes.EmailInUse,
                $"With ${nameof(user.Email)}: '{user.Email}' already exists"));
        }

        var userTable = new User(Guid.NewGuid(), user.Email, user.Role, user.Firstname, user.Lastname,
            user.PhoneNumber)
        {
            UserType = UserType.User
        };
        userTable.SetPassword(user.Password, _passwordHasher);
        await _genericRepository.AddAsync(userTable);
        var authResult =
            new AuthResult(
                new SignedUp(userTable.FirstName, userTable.LastName, userTable.PhoneNumber, userTable.Email),
                _jwtTokenGenerator.GenerateToken(new TokenRequest(user.Email, user.PhoneNumber, user.Firstname,
                    user.Lastname, user.Role)));
        return new Either<AuthResult, AuthException>(authResult);
    }
}