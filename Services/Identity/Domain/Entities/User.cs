using Authentication;
using Authentication.Enum;
using Microsoft.AspNetCore.Identity;
using Shared.BaseEntity;
using Shared.Models;
using Shared.Models.Constants;

namespace Identity.Domain.Entities;

public class User : Protectable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Role Role { get; set; }

    public List<string> Authorities { get; set; }
    public string Locale { get; set; }

    public User()
    {
    }

    public User(string firstName, string lastName, string email, string? phoneNumber, string? locale = null)
    {
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber ?? "_";
        Locale = locale ?? "UZ";
    }

    public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new AuthException(Codes.InvalidPassword, $"Password can't be empty.");


        PasswordHash = passwordHasher.HashPassword(this, password);
    }

    public TokenRequest ToTokenRequest() => new(Id, Email, PhoneNumber, FirstName, LastName, Roles.Customer.ToString());

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;


    private static bool PhoneNumberValidate(string input)
        => Regexs.NumberRegex.IsMatch(input);

    public bool ValidatePassword(string password, IPasswordHasher<User> passwordHasher)
        => passwordHasher.VerifyHashedPassword(this, PasswordHash, password) != PasswordVerificationResult.Failed;
}