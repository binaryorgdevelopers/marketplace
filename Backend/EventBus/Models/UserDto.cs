namespace EventBus.Models;

public class UserDto
{
    public Guid? userId = Guid.Empty;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    public UserDto()
    {
    }

    public UserDto(Guid? userId, string firstName, string lastName, string email, string role)
    {
        this.userId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
    }
}

public class UserToken
{
    public readonly string Token = string.Empty;

    public UserToken()
    {
    }

    public UserToken(string token)
    {
        Token = token;
    }
}