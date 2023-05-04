using EventBus.Models;

namespace Inventory.Api.Authentication;

public interface IAuth
{
    Task<UserDto> ValidateToken(UserToken user);
}