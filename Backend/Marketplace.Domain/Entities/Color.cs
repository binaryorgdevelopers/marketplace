using Marketplace.Domain.Abstractions;

namespace Marketplace.Domain.Entities;

public class Color : IIdentifiable
{
    public Guid Id { get; set; }
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
        Title = title;
        Value = value;
    }
}