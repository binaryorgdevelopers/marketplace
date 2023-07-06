namespace Identity.Models;

public record BindCardToUserCommand(Guid UserId, CardCreateCommand Card);