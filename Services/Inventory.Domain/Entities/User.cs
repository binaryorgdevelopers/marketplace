using Authentication;
using Authentication.Enum;
using Inventory.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using Shared.Models;
using Shared.Models.Constants;

namespace Inventory.Domain.Entities;

public class User : IIdentifiable, ICommon, IProtectable, IPrivacyProvider<User>
{
    public User()
    {
    }


    public User(Guid id, string email, string role)
    {
        if (!Regexs.EmailRegex.IsMatch(email))
            throw new AuthException(Codes.InvalidEmail, $"Invalid email :'{email}.'");

        // Entities.Role.TryValidateRole(role, out var output);
        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        // Role = output;
        Email = email;
    }

    public User(Guid id, string email, string role, string firstName, string lastName, string phoneNumber)
    {
        if (!Regexs.EmailRegex.IsMatch(email))
            throw new AuthException(Codes.InvalidEmail, $"Invalid email :'{email}.'");


        if (!PhoneNumberValidate(phoneNumber))
            throw new AuthException(Codes.InvalidPhoneNumber, $"Invalid phone number '{phoneNumber}'.");

        // Entities.Role.TryValidateRole(role, out var output);

        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        // Role = output;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public Roles Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }


    public List<Blob> Files { get; set; }


    public int ShopId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }
    public Guid Id { get; set; }

    public TokenRequest ToTokenRequest()
    {
        return new(Id, Email, PhoneNumber, FirstName, LastName, Role.ToString());
    }


    public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new AuthException(Codes.InvalidPassword, "Password can't be empty.");

        PasswordHash = passwordHasher.HashPassword(this, password);
    }

    public bool PhoneNumberValidate(string input)
    {
        return Regexs.NumberRegex.IsMatch(input);
    }

    public bool ValidatePassword(string password, IPasswordHasher<User> passwordHasher)
    {
        return passwordHasher.VerifyHashedPassword(this, PasswordHash, password) != PasswordVerificationResult.Failed;
    }

    public string PhoneNumber { get; set; }
    public string PasswordHash { get; private set; }
    public string Email { get; set; }
}