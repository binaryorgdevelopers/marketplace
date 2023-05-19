namespace Identity.Models;

public record UserCreateCommand(string PhoneNumber, string Email, string Password, string Firstname, string Lastname,
    string Role = null);