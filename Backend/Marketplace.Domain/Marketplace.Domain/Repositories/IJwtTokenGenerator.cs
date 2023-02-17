using Marketplace.Domain.Models;

namespace Marketplace.Domain.Repositories;

public interface IJwtTokenGenerator
{
    JsonWebToken GenerateToken(TokenRequest user);
}