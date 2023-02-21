using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public class UserRead
{
    public UserRead(Guid id, DateTime createdAt, DateTime updatedAt, string role, string firstName, string lastName,
        string phoneNumber, string email, IEnumerable<Shop> shops)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Role = role;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        Shops = shops;
    }

    public UserRead(Guid id, DateTime createdAt, DateTime updatedAt, string role, string firstName, string lastName,
        string phoneNumber, string email)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Role = role;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public IEnumerable<Shop> Shops { get; set; }
}