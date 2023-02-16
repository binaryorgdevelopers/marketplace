namespace Marketplace.Application.Common.Messages;

public record JsonWebToken(string Token, int ExpiresIn);