namespace Marketplace.Application.Common.Messages.Messages;

public class CustomerRead
{
    public CustomerRead(string firstName, string lastName, string locale, string username, string[] authorities, string phoneNumber, string email, bool active)
    {
        FirstName = firstName;
        LastName = lastName;
        Locale = locale;
        Username = username;
        Authorities = authorities;
        PhoneNumber = phoneNumber;
        Email = email;
        Active = active;
    }

    public CustomerRead()
    {
        
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Locale { get; set; }
    public string Username { get; set; }
    public string[] Authorities { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public  bool Active { get; set; }
}