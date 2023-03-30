using Marketplace.Domain.Abstractions;

namespace Marketplace.Domain.Entities;

public class Characteristics : IIdentifiable
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public ICollection<Color> Values { get; set; }

    public Product Product { get; set; }
    public Guid ProductId { get; set; }

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

    public static Characteristics Create(string title, ICollection<Color> values, Guid productId) =>
        new(title, values, productId);
}