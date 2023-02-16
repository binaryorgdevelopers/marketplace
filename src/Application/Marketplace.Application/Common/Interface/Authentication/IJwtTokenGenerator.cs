using Marketplace.Application.Common.Messages.Commands;

namespace Marketplace.Application.Common.Interface.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserCreate user);
}