using Inventory.Domain.Abstractions;

namespace Inventory.Domain.Entities;

public class Characteristics : IIdentifiable
{
    public Characteristics(ICollection<Color> values, string title)
    {
        Values = values;
        Title = title;
    }

    public Characteristics()
    {
    }

    private Characteristics(Guid id, string title, ICollection<Color> colors)
    {
        Id = id;
        Title = title;
        Values = colors;
    }

    private Characteristics(string title, ICollection<Color> values, Guid productId)
    {
        Title = title;
        Values = values;
        ProductId = productId;
    }

    public string Title { get; set; }
    public ICollection<Color> Values { get; set; }

    public Product Product { get; set; }
    public Guid ProductId { get; set; }
    public Guid Id { get; set; }

    public static Characteristics Create(string title, ICollection<Color> values, Guid productId)
    {
        return new(title, values, productId);
    }
}