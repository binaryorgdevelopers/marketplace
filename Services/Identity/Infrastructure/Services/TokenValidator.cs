using Authentication;
using EventBus.Models;
using Identity.Infrastructure.Repositories;

namespace Identity.Infrastructure.Services;

public class TokenValidator : ITokenValidator
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserRepository _userRepository;

    public TokenValidator(ITokenProvider tokenProvider, IUserRepository userRepository)
    {
        _tokenProvider = tokenProvider;
        _userRepository = userRepository;
    }

    public Task<UserDto?> ValidateToken(UserToken userToken)
        => Task.Run(async () =>
        {
            Guid? guid = _tokenProvider.ValidateJwtToken(userToken.Token[7..]);
            if (guid is null) return null;
            var user = await _userRepository.GetUserById(guid.Value);
            return new UserDto(user.Id, user.FirstName, user.LastName, user.Email, user.Role.Name);
        });
}