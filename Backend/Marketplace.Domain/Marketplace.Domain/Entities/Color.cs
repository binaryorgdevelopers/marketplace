using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Constants;

namespace Marketplace.Domain.Entities;

public class Color : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Value { get; set; }

    public Color(Guid id, string title, string value)
    {
        Id = id;
        Title = title;
        Value = value;
    }

    public Color()
    {
        
    }
    public Characteristics Characteristics { get; set; }
    public Guid CharId { get; set; }

    public Color(string title, string value)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new Exception(Messages.CantNull(string.Empty));
        if (string.IsNullOrWhiteSpace(value)) throw new Exception(Messages.CantNull(string.Empty));
    }
}