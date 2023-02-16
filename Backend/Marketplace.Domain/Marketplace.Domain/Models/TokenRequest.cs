namespace Marketplace.Domain.Models;

public record  TokenRequest(string Email ,string PhoneNumber,string FirstName,string LastName,string Role);