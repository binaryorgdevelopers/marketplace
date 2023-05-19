using Authentication;
using Authentication.Enum;
using Inventory.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using Shared.Models;
using Shared.Models.Constants;

namespace Inventory.Domain.Entities;

public class Customer : ICommon, IIdentifiable, IProtectable
{
    //ICommon
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    //IIdentifiable
    public Guid Id { get; set; }

    //IProtectable
    public string PasswordHash { get; private set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime LastSession { get; set; }

    public bool Active { get; private set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    // public string? Locale { get; set; } = string.Empty;
    public string Username { get; set; }
    public string[] Authorities { get; set; }

    public BillingAddress BillingAddress { get; set; }
    public List<CardDetail> CardDetails { get; set; }


    public Customer()
    {
    }

    public Customer(Guid id, string phoneNumber, string email, string firstName, string lastName, string username)
    {
        if (!Regexs.EmailRegex.IsMatch(email))
            throw new AuthException(Codes.InvalidEmail, $"Invalid email :'{email}.'");


        if (!PhoneNumberValidate(phoneNumber))
            throw new AuthException(Codes.InvalidPhoneNumber, $"Invalid phone number '{phoneNumber}'.");


        Id = id;
        PhoneNumber = phoneNumber;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Active = true;
    }

    public static Customer Create(Guid id, string phoneNumber, string email, string firstName, string lastName,
        string username) => new(id, phoneNumber, email, firstName, lastName, username);

    public void SetPassword(string password, IPasswordHasher<Customer> passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new AuthException(Codes.InvalidPassword, $"Password can't be empty.");


        PasswordHash = passwordHasher.HashPassword(this, password);
    }

    public TokenRequest ToTokenRequest() => new(Id, Email, PhoneNumber, FirstName, LastName, Roles.Customer.ToString());

    public void ChangeStatus() => Active = !Active;


    private static bool PhoneNumberValidate(string input)
        => Regexs.NumberRegex.IsMatch(input);

    public bool ValidatePassword(string password, IPasswordHasher<Customer> passwordHasher)
        => passwordHasher.VerifyHashedPassword(this, PasswordHash, password) != PasswordVerificationResult.Failed;
}