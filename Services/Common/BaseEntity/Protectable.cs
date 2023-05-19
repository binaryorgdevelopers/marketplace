namespace Shared.BaseEntity;

public class Protectable : BaseEntity
{
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}