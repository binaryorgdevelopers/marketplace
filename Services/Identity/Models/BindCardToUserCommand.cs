namespace Identity.Models;

public record BindCardToUserCommand(string Email, CardCreateCommand Card);