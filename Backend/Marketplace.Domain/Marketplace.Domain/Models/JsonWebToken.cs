namespace Marketplace.Domain.Models;

public record JsonWebToken(string Token, string ExpiresIn);