using Authentication.Enum;
using Inventory.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using Shared.Models;
using Shared.Models.Constants;

namespace Inventory.Domain.Entities;

public class Seller : IIdentifiable, ICommon, IProtectable, IPrivacyProvider<Seller>
{
    //IIdentifiable
    public Guid Id { get; set; }

    //ICommon
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }

    //IProtectable
    public string PasswordHash { get; private set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }

    public string Title { get; set; }
    public string Link { get; set; }
    public string Banner { get; set; }
    public string Avatar { get; set; }
    public string Description { get; set; }
    public string Info { get; set; }

    public bool HasCharityProducts { get; set; }
    public double Ratings { get; set; }
    public double Reviews { get; set; }
    public int Orders { get; set; }
    public bool Official { get; set; }
    public int TotalProducts { get; set; }

    public List<Category> Categories { get; set; } = new();
    public List<Product> Products { get; set; } = new();


    public Seller()
    {
    }

    public Seller(Guid id, string phoneNumber, string email, string title, string description,
        string info, string username, string firstName, string lastName, string? link,
        string? banner, string? avatar)
    {
        Id = id;
        PhoneNumber = phoneNumber;
        Email = email;
        Title = title;
        Description = description;
        Info = info;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Banner = banner ?? string.Empty;
        Avatar = avatar ?? string.Empty;
        Link = link ?? username.Trim();
    }


    public bool ValidatePassword(string password, IPasswordHasher<Seller> passwordHasher) =>
        passwordHasher.VerifyHashedPassword(this, PasswordHash, password) != PasswordVerificationResult.Failed;


    public void SetPassword(string password, IPasswordHasher<Seller> passwordHasher)
    {
        PasswordHash = passwordHasher.HashPassword(this, password);
    }

    public bool PhoneNumberValidate(string input)
        => Regexs.NumberRegex.IsMatch(input);


    public TokenRequest ToTokenRequest() => new(Id, Email, PhoneNumber, FirstName, LastName, Roles.Seller.ToString());
}