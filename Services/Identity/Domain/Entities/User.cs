using Authentication;
using Identity.Models.Dtos;
using Identity.Models.Messages;
using Microsoft.AspNetCore.Identity;
using Shared.BaseEntity;
using Shared.Models;
using Shared.Models.Constants;

namespace Identity.Domain.Entities;

public class User : Protectable
{
    public User()
    {
    }

    public User(string firstName, string lastName, string email, string? phoneNumber, Guid roleId,
        string? locale = null)
    {
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber ?? "_";
        Locale = locale ?? "UZ";
        RoleId = roleId;
        Authorities = new[] { "UZ" };
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public string[] Authorities { get; set; }
    public string Locale { get; set; }
    public List<CardDetail>? Cards { get; set; }

    public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new AuthException(Codes.InvalidPassword, "Password can't be empty.");


        PasswordHash = passwordHasher.HashPassword(this, password);
    }

    public TokenRequest ToTokenRequest()
    {
        return new(Id, Email, PhoneNumber, FirstName, LastName, Role.Name);
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public UserDto ToDto() => new(Id, FirstName, Email, LastName, this.Role.ToDto(), Authorities, Locale,
        new List<CardReadDto>());

    private static bool PhoneNumberValidate(string input)
    {
        return Regexs.NumberRegex.IsMatch(input);
    }

    public bool ValidatePassword(string password, IPasswordHasher<User> passwordHasher)
    {
        return passwordHasher.VerifyHashedPassword(this, PasswordHash, password) != PasswordVerificationResult.Failed;
    }
}