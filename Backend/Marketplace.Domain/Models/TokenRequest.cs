namespace Marketplace.Domain.Models;

public record  TokenRequest(Guid Id,string Email ,string PhoneNumber,string FirstName,string LastName,string Role);