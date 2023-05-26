using Authentication;
using EventBus.Models;
using Identity.Services;

namespace Marketplace.Infrastructure.Services;

public class ValidatorService : ITokenValidator
{
    private readonly AuthService.AuthServiceClient _authServiceClient;

    public ValidatorService(AuthService.AuthServiceClient authServiceClient)
    {
        _authServiceClient = authServiceClient;
    }

    public Task<UserDto?> ValidateToken(UserToken user) =>
        Task.Run(() =>
        {
            var userToken = new GrpcToken
            {
                Token = user.Token
            };
            var result = _authServiceClient.ValidateToken(userToken);
            if (result.Code == 404 || result.User is null) return null;
            Guid.TryParse(result.User.Id, out var guid);

            return new UserDto(guid, result.User.FirstName, result.User.LastName, result.User.Email, result.User.Role);
        });
}