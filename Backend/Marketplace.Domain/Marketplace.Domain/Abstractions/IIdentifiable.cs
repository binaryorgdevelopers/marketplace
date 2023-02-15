namespace Marketplace.Domain.Abstractions;

public interface IIdentifiable
{
    public Guid Id { get; set; }
}