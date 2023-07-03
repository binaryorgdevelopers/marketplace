namespace Identity.Models;

public record UserCreateCommand(string PhoneNumber, string Email, string Password, string Firstname, string Lastname,
    Guid? RoleId = null, string? RoleName = null);