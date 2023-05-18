using EventBus.Models;

namespace Authentication;

public interface ITokenValidator
{
    Task<UserDto?> ValidateToken(UserToken user);
}