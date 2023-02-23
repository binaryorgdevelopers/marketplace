using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Queries.Query;

public class UserSignInQuery : IUserSignInQuery
{
    private readonly IGenericRepository<User> _genericRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserSignInQuery(IJwtTokenGenerator tokenGenerator,
        IPasswordHasher<User> passwordHasher, IGenericRepository<User> genericRepository)
    {
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
        _genericRepository = genericRepository;
    }

    public async Task<Either<AuthResult, AuthException>> SignIn(SignInCommand signInCommand)
    {
        var user = await _genericRepository.GetAsync(e => e.Email == signInCommand.Email);
        if (user is null)
        {
            return new Either<AuthResult, AuthException>(new AuthException(Codes.UserNotFound,
                $"User with email :'{signInCommand.Email}' not found"));
        }

        var validationResult = user.ValidatePassword(signInCommand.Password, _passwordHasher);
        if (!validationResult)
        {
            return new Either<AuthResult, AuthException>(new AuthException(Codes.InvalidCredential,
                "Invalid password"));
        }

        Authorized authorized =
            new Authorized(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.Email, user.Role);

        return new Either<AuthResult, AuthException>(new AuthResult(authorized,
            _tokenGenerator.GenerateToken(user.ToTokenRequest())));
    }
}