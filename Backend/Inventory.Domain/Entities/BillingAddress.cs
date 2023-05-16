using Inventory.Domain.Abstractions;

#pragma warning disable CS8618

namespace Inventory.Domain.Entities;

public class BillingAddress : IIdentifiable
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }

    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; }

    private BillingAddress(
        string firstName, string lastName, string streetAddress, string city,
        string state, string zipCode, string country, Guid customerId)
    {
        FirstName = firstName;
        LastName = lastName;
        StreetAddress = streetAddress;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
        CustomerId = customerId;
    }

    public BillingAddress()
    {
    }

    public static BillingAddress Create(
        string firstName, string lastName, string streetAddress, string city, string state, string zipCode,
        string country, Guid customerId) =>
        new(
            firstName,
            lastName,
            streetAddress,
            city,
            state,
            zipCode,
            country,
            customerId
        );
}