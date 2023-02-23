namespace Marketplace.Application.Common.Messages.Messages;

public record CustomerRead(
    string FirstName,
    string LastName,
    string Locale,
    string Username,
    string[] Authorities,
    string PhoneNumber,
    string Email,
    bool Active
);