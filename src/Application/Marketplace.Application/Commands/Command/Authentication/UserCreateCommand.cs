using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Commands.ICommand.Authentication;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Marketplace.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Commands.Command.Authentication;

public class UserCreateCommand : IUserCreateCommand
{
    private readonly IGenericRepository<User> _genericRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserCreateCommand(IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator, IGenericRepository<User> genericRepository)
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _genericRepository = genericRepository;
    }

    public async Task<Either<AuthResult, AuthException>> CreateUser(SignUp user)
    {
        var searchResult = await _genericRepository.GetAsync(e => e.Email == user.Email);
        if (searchResult is not null)
        {
            return new Either<AuthResult, AuthException>(new AuthException(Codes.EmailInUse,
                $"With ${nameof(user.Email)}: '{user.Email}' already exists"));
        }

        User userTable = new User(Guid.NewGuid(), user.Email, user.Role, user.Firstname, user.Lastname,
            user.PhoneNumber);
        userTable.SetPassword(user.Password, _passwordHasher);
        await _genericRepository.AddAsync(userTable);
        var authResult =
            new AuthResult(
                new Authorized(
                    userTable.Id,
                    userTable.FirstName,
                    userTable.LastName,
                    userTable.PhoneNumber,
                    userTable.Email,
                    userTable.Role),
                _jwtTokenGenerator.GenerateToken(new TokenRequest(userTable.Id,userTable.Email, userTable.PhoneNumber, userTable.FirstName,
                    userTable.LastName, user.Role)));
        return new Either<AuthResult, AuthException>(authResult);
    }
}