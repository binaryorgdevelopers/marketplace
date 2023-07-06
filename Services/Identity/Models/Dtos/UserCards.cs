namespace Identity.Models.Messages;

public record UserCards(Guid UserId, IEnumerable<CardReadDto> Cards);