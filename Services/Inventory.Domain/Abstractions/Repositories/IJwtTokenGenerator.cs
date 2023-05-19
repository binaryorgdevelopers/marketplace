using Shared.Models;

namespace Inventory.Domain.Abstractions.Repositories;

public interface IJwtTokenGenerator
{
    JsonWebToken GenerateToken(TokenRequest user);

    Guid? ValidateJwtToken(string? token);
}