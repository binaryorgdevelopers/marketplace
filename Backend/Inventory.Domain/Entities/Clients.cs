using Inventory.Domain.Abstractions;
using Inventory.Domain.Models.Constants;

namespace Inventory.Domain.Entities;

public class Clients : IIdentifiable, ICommon, IProtectable
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Roles Role { get; set; }

    public Clients()
    {
    }

    public Clients(Guid id, string passwordHash, string phoneNumber, string email, Roles role,
        string firstName, string lastName)
    {
        Id = id;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        Email = email;
        Role = role;
        FirstName = firstName;
        LastName = lastName;
    }
}