using Marketplace.Domain.Abstractions;

namespace Marketplace.Domain.Entities;

public class Characteristics : IIdentifiable
{
    public Guid Id { get; set; } 
    public string Title { get; set; }
    public List<Color> Values { get; set; }
    
    public Product Product { get; set; }
    public Guid ProductId { get; set; }

    public Characteristics(List<Color> values, string title)
    {
        Values = values;
        Title = title;
    }

    public Characteristics()
    {
        
    }
    public Characteristics(Guid id, string title, List<Color> colors)
    {
        Id = id;
        Title = title;
        Values = colors;
    }
}