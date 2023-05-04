

// ReSharper disable All

using Authentication.Enum;

namespace Marketplace.Application.Common.Messages.Events;

public record Authorized 
{
    public Authorized(Guid id, string firstname, string lastname, string phoneNumber, string email, Roles role)
    {
        Id = id;
        Firstname = firstname;
        Lastname = lastname;
        PhoneNumber = phoneNumber;
        Email = email;
        Role = Enum.GetName(typeof(Roles), role)!;
    }

    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}