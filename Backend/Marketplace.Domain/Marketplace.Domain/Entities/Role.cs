using Marketplace.Domain.Abstractions;

namespace Marketplace.Domain.Entities;

public class Role : IIdentifiable, ICommon
{
    public const string User = "user";
    public const string Admin = "admin";

    public static bool TryValidateRole(string input, out string role)
    {
        if (string.IsNullOrWhiteSpace(input)) role = User;
        else if (input.Equals(User, StringComparison.OrdinalIgnoreCase) ||
                 input.Equals(Admin, StringComparison.OrdinalIgnoreCase))
            role = input.ToLowerInvariant();
        else role = string.Empty;
        return role is User or Admin;
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public string Permissions { get; set; }
    public string Description { get; set; }
}