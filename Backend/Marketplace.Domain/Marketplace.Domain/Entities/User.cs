using System.Text.RegularExpressions;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Exceptions;
using Marketplace.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Domain.Entities;

public class User : IIdentifiable, ICommon
{
    private static readonly Regex EmailRegex = new(Regexs.EmailRegexPattern, RegexOptions.IgnoreCase |
                                                                             RegexOptions.Compiled |
                                                                             RegexOptions.CultureInvariant);

    private static readonly Regex NumberRegex = new(Regexs.PhoneNumberRegexPatter);

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserType UserType { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; private set; }

    public List<Shop> Shops { get; set; }

    public User()
    {
    }

    public string Email { get; set; }

    public int ShopId { get; set; }


    public User(Guid id, string email, string role)
    {
        if (!EmailRegex.IsMatch(email))
        {
            throw new AuthException(Codes.InvalidEmail, $"Invalid email :'{email}.'");
        }

        if (Entities.Role.TryValidateRole(role, out var output))
        {
            throw new AuthException(Codes.InvalidRole, $"Invalid role: '{role}'.");
        }

        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Role = output;
        Email = email;
    }

    public User(Guid id, string email, string role, string firstName, string lastName, string phoneNumber)
    {
        if (!EmailRegex.IsMatch(email))
        {
            throw new AuthException(Codes.InvalidEmail, $"Invalid email :'{email}.'");
        }

        if (Entities.Role.TryValidateRole(role, out var output))
        {
            throw new AuthException(Codes.InvalidRole, $"Invalid role: '{role}'.");
        }

        if (!PhoneNumberValidate(phoneNumber))
        {
            throw new AuthException(Codes.InvalidPhoneNumber, $"Invalid phone number '{phoneNumber}'.");
        }

        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Role = output;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public TokenRequest ToTokenRequest => new(Email, PhoneNumber, FirstName, LastName, Role);


    public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new AuthException(Codes.InvalidPassword, $"Password can't be empty.");
        }

        PasswordHash = passwordHasher.HashPassword(this, password);
    }

    public static bool PhoneNumberValidate(string input)
        => NumberRegex.IsMatch(input);

    public bool ValidatePassword(string password, IPasswordHasher<User> passwordHasher)
        => passwordHasher.VerifyHashedPassword(this, PasswordHash, password) != PasswordVerificationResult.Failed;
}