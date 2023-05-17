using Authentication;
using Authentication.Enum;
using Inventory.Domain.Abstractions;
using Inventory.Domain.Models;
using Inventory.Domain.Models.Constants;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Domain.Entities;

public class User : IIdentifiable, ICommon, IProtectable, IPrivacyProvider<User>
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }
    
    public Roles Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string PhoneNumber { get; set; }
    public string PasswordHash { get; private set; }
    public string Email { get; set; }


    public List<Blob> Files { get; set; }

    public User()
    {
    }


    public int ShopId { get; set; }


    public User(Guid id, string email, string role)
    {
        if (!Regexs.EmailRegex.IsMatch(email))
        {
            throw new AuthException(Codes.InvalidEmail, $"Invalid email :'{email}.'");
        }

        Entities.Role.TryValidateRole(role, out var output);
        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Role = output;
        Email = email;
    }

    public User(Guid id, string email, string role, string firstName, string lastName, string phoneNumber)
    {
        if (!Regexs.EmailRegex.IsMatch(email))
        {
            throw new AuthException(Codes.InvalidEmail, $"Invalid email :'{email}.'");
        }


        if (!PhoneNumberValidate(phoneNumber))
        {
            throw new AuthException(Codes.InvalidPhoneNumber, $"Invalid phone number '{phoneNumber}'.");
        }

        Entities.Role.TryValidateRole(role, out var output);

        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Role = output;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public TokenRequest ToTokenRequest() =>
        new(Id, Email, PhoneNumber, FirstName, LastName, Role.ToString());


    public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new AuthException(Codes.InvalidPassword, $"Password can't be empty.");
        }

        PasswordHash = passwordHasher.HashPassword(this, password);
    }

    public bool PhoneNumberValidate(string input)
        => Regexs.NumberRegex.IsMatch(input);

    public bool ValidatePassword(string password, IPasswordHasher<User> passwordHasher)
        => passwordHasher.VerifyHashedPassword(this, PasswordHash, password) != PasswordVerificationResult.Failed;
}