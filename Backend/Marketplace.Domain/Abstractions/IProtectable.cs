namespace Marketplace.Domain.Abstractions;

public interface IProtectable
{
    public string PasswordHash { get; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}