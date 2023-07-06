using Inventory.Domain.Abstractions;

#pragma warning disable CS8618

namespace Inventory.Domain.Entities;

public class BillingAddress : IIdentifiable
{
    private BillingAddress(
        string firstName, string lastName, string streetAddress, string city,
        string state, string zipCode, string country, Guid userId)
    {
        FirstName = firstName;
        LastName = lastName;
        StreetAddress = streetAddress;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
        UserId = userId;
    }

    public BillingAddress()
    {
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }

    public Guid UserId { get; set; }

    public Guid Id { get; set; }

    public static BillingAddress Create(
        string firstName, string lastName, string streetAddress, string city, string state, string zipCode,
        string country, Guid userId)
    {
        return new(
            firstName,
            lastName,
            streetAddress,
            city,
            state,
            zipCode,
            country,
            userId
        );
    }
}