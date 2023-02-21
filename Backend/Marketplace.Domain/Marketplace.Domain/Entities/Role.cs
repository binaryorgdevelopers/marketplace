using Marketplace.Domain.Constants;
using static System.Enum;

namespace Marketplace.Domain.Entities;

public static class Role
{
    public const string User = "User";
    public const string Admin = "Admin";

    public static void TryValidateRole(string input, out Roles role)
    {
        if (!TryParse(input, true, out role)) role = Roles.User;
    }

    // public Guid Id { get; set; }
    // public DateTime CreatedAt { get; set; }
    // public DateTime UpdatedAt { get; set; }
    // public string Name { get; set; }
    // public string Permissions { get; set; }
    // public string Description { get; set; }
}