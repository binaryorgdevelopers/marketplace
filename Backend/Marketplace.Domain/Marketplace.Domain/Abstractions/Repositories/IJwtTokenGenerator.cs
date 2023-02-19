using Marketplace.Domain.Models;

namespace Marketplace.Domain.Abstractions.Repositories;

public interface IJwtTokenGenerator
{
    JsonWebToken GenerateToken(TokenRequest user);
}