namespace Identity.Models.Messages;

public record CardReadDto(string CardNumber, string ExpiryMonth, string ExpiryYear, string Cvv, string CardHolderName);