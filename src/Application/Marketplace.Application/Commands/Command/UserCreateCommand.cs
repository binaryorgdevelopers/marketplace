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
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserCreateCommand(IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<Either<AuthResult, AuthException>> CreateUser(SignUp user)
    {
        var searchResult = await _userRepository.GetAsync(user.Email);
        if (searchResult is not null)
        {
            return new Either<AuthResult, AuthException>(new AuthException(Codes.EmailInUse,
                $"With ${nameof(user.PhoneNumber)}: '{user.PhoneNumber}' already exists"));
        }

        User userTable = new User(Guid.NewGuid(), user.Email, user.Role, user.Firstname, user.Lastname,
            user.PhoneNumber)
        {
            UserType = UserType.User
        };
        userTable.SetPassword(user.Password, _passwordHasher);
        await _userRepository.AddAsync(userTable);
        var authResult =
            new AuthResult(
                new SignedUp(userTable.FirstName, userTable.LastName, userTable.PhoneNumber, userTable.Email),
                _jwtTokenGenerator.GenerateToken(new TokenRequest(user.Email, user.PhoneNumber, user.Firstname,
                    user.Lastname,user.Role)));
        return new Either<AuthResult, AuthException>(authResult);
    }
}