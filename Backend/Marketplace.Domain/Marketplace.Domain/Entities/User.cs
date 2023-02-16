using System.Text.RegularExpressions;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Domain.Entities;

public class User : IIdentifiable, ICommon
{
    private static readonly Regex EmailRegex = new Regex(Regexs.EmailRegexPattern, RegexOptions.IgnoreCase |
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

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

        if (!Entities.Role.IsValid(role))
        {
            throw new AuthException(Codes.InvalidRole, $"Invalid role: '{role}'.");
        }

        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Role = role.ToLowerInvariant();
        Email = email;
    }

    public User(Guid id, string email, string role, string firstName, string lastName, string phoneNumber)
    {
        if (!EmailRegex.IsMatch(email))
        {
            throw new AuthException(Codes.InvalidEmail, $"Invalid email :'{email}.'");
        }

        if (!Entities.Role.IsValid(role))
        {
            throw new AuthException(Codes.InvalidRole, $"Invalid role: '{role}'.");
        }

        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Role = role.ToLowerInvariant();
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }


    public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new AuthException(Codes.InvalidPassword, $"Password can't be empty.");
        }

        PasswordHash = passwordHasher.HashPassword(this, password);
    }

    public bool ValidatePassword(string password, IPasswordHasher<User> passwordHasher)
        => passwordHasher.VerifyHashedPassword(this, PasswordHash, password) != PasswordVerificationResult.Failed;
}