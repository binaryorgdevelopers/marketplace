using Inventory.Domain.Abstractions;

namespace Inventory.Domain.Entities;

public class Color : IIdentifiable
{
    public Color(Guid id, string title, string value)
    {
        Id = id;
        Title = title;
        Value = value;
    }

    public Color()
    {
    }

    public Color(string title, string value)
    {
        Title = title;
        Value = value;
    }

    public string Title { get; set; }
    public string Value { get; set; }

    public Characteristics Characteristics { get; set; }
    public Guid CharId { get; set; }
    public Guid Id { get; set; }
}