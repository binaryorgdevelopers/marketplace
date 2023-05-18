using Inventory.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public class UserDto : BaseDto<UserDto, User>
{
    public UserDto(Guid id, DateTime createdAt, DateTime updatedAt, string role, string firstName, string lastName,
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

    public UserDto()
    {
        
    }
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}