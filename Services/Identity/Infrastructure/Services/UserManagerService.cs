
using Identity.Infrastructure.Repositories;
using Identity.Models;
using Shared.Messages;
using Shared.Models;
using Shared.Models.Constants;

namespace Identity.Infrastructure.Services;

public class UserManagerService
{
    private readonly IUserRepository _repository;
    private readonly ITokenProvider _tokenProvider;

    public UserManagerService(IUserRepository repository, ITokenProvider tokenProvider)
    {
        _repository = repository;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<AuthResult>> Register(UserCreateCommand createCommand)
    {
        var saveResult = await _repository.AddAsync(createCommand, new CancellationTokenSource().Token);
        if (saveResult is null)
        {
            return Result.Failure<AuthResult>(new Error(Codes.InvalidCredential, "Error while saving new User"));
        }

        var token = await _tokenProvider.GenerateToken(saveResult.ToTokenRequest());
        return Result.Create(new AuthResult(new Authorized(saveResult.Id, saveResult.FirstName, saveResult.LastName,
            saveResult.PhoneNumber,
            saveResult.Email, ""), new JsonWebToken(token.Token, token.ExpiresAt.ToString())));
    }
}