using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Constants;

namespace Marketplace.Domain.Entities;

public class User : IIdentifiable,ICommon
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserType UserType { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; } = string.Empty;
    public int ShopId { get; set; }
}