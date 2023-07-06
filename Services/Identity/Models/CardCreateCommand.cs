namespace Identity.Models;

public record CardCreateCommand(string CardNumber,string ExpiryMonth,string ExpiryYear,string Cvv,string CardHolderName);