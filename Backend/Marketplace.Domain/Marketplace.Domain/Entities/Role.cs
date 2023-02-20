using Marketplace.Domain.Abstractions;
using static System.Enum;

namespace Marketplace.Domain.Entities;

public class Role : IIdentifiable, ICommon
{
    public const string User = "user";
    public const string Admin = "admin";

    public static bool TryValidateRole(string input, out RoleEnum role)
    {
        TryParse<RoleEnum>(input, out var parsedEnum);
        var isExist = IsDefined(typeof(RoleEnum), input);
        if (!isExist && input is User or Admin) role = parsedEnum;
        else role = RoleEnum.User;
        return role is RoleEnum;
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public string Permissions { get; set; }
    public string Description { get; set; }
}

public enum RoleEnum
{
    Admin,
    User
}