using Marketplace.Application.Common;
using Marketplace.Application.Common.Interface.Authentication;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Marketplace.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Queries;

public class UserSignInQuery : IUserSignInQuery
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserSignInQuery(IUserRepository userRepository, IJwtTokenGenerator tokenGenerator,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Either<AuthResult, AuthException>> SignIn(SignIn signIn)
    {
        var user = await _userRepository.GetAsync(signIn.Email);
        if (user is null)
        {
            return new Either<AuthResult, AuthException>(new AuthException(Codes.UserNotFound,
                $"User with email :'{signIn.Email}' not found"));
        }

        var validationResult = user.ValidatePassword(signIn.Password, _passwordHasher);
        if (!validationResult)
        {
            return new Either<AuthResult, AuthException>(new AuthException(Codes.InvalidCredential,
                "Invalid password"));
        }

        SignedUp signedUp = new SignedUp(user.FirstName, user.LastName, user.PhoneNumber, user.Email);

        return new Either<AuthResult, AuthException>(new AuthResult(signedUp,
            _tokenGenerator.GenerateToken(user.ToTokenRequest)));
    }
}