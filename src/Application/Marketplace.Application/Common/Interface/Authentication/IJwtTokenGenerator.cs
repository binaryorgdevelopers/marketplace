using Marketplace.Application.Common.Messages;
using Marketplace.Domain.Models;

namespace Marketplace.Application.Common.Interface.Authentication;

public interface IJwtTokenGenerator
{
    JsonWebToken GenerateToken(TokenRequest user);
}