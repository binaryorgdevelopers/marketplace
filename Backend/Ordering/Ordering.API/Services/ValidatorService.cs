using Authentication;
using EventBus.Models;
using Ordering.Application;
using Ordering.Infrastructure.Services;

namespace Ordering.API.Services;

public class ValidatorService : ITokenValidator
{
    private readonly Marketplace.Ordering.Ordering.API.AuthService.AuthServiceClient _authServiceClient;
    private readonly StateService _stateService;

    public ValidatorService(Marketplace.Ordering.Ordering.API.AuthService.AuthServiceClient authServiceClient,
        StateService stateService)
    {
        _authServiceClient = authServiceClient;
        _stateService = stateService;
    }

    public Task<UserDto?> ValidateToken(UserToken user) =>
        Task.Run(() =>
        {
            var userToken = new Marketplace.Ordering.Ordering.API.UserToken
            {
                Token = user.Token
            };
            var result = _authServiceClient.ValidateToken(userToken);
            if (result.Code == 400) return null;

            if (Guid.TryParse(result.User.Id, out var guid))
            {
                _stateService.CurrentUserId = guid;
            }

            return new UserDto(guid, result.User.FirstName, result.User.LastName, result.User.Email, result.User.Role);
        });
}