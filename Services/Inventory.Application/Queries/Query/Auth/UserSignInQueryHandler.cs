using Davr.Services.Marketplace.Application.Common.Messages.Events;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Inventory.Domain.Models.Constants;
using Inventory.Domain.Shared;
using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Queries.Query.Auth;

public class UserSignInQueryHandler : ICommandHandler<UserSignInCommand,AuthResult>
{
    private readonly IGenericRepository<User> _genericRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserSignInQueryHandler(IJwtTokenGenerator tokenGenerator,
        IPasswordHasher<User> passwordHasher, IGenericRepository<User> genericRepository)
    {
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
        _genericRepository = genericRepository;
    }

    public async Task<Result<AuthResult>> Handle(UserSignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _genericRepository.GetAsync(e => e.Email == request.Email);
        if (user is null)
        {
            return Result.Failure<AuthResult>(new Error(Codes.UserNotFound,
                $"User with email :'{request.Email}' not found"));
        }

        var validationResult = user.ValidatePassword(request.Password, _passwordHasher);
        if (!validationResult)
        {
            return Result.Failure<AuthResult>(new Error(Codes.InvalidCredential,
                "Invalid password"));
        }

        Authorized authorized =
            new Authorized(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.Email, user.Role);

        return Result.Success(new AuthResult(authorized,
            _tokenGenerator.GenerateToken(user.ToTokenRequest())));
    }
}