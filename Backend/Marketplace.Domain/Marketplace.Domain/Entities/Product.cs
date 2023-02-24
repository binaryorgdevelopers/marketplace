using Marketplace.Domain.Abstractions;

namespace Marketplace.Domain.Entities;

public class Product : IIdentifiable, ICommon
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }

    public string[]? Attributes { get; set; }
    public IEnumerable<Badge> Badges { get; set; }
    public string? Synonyms { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public Seller Seller { get; set; }
    public IEnumerable<Blob> Photos { get; set; }
    public IEnumerable<Characteristics> Characteristics { get; set; }

    public Guid SellerId { get; set; }
    public Guid CategoryId { get; set; }

    public Product()
    {
    }

    public Product( IEnumerable<Badge> badges, string title,
        string description, Guid categoryId, Guid sellerId, IEnumerable<Blob> photos,
        IEnumerable<Characteristics> characteristics)
    {
        Id = Guid.NewGuid();
        // Attributes = attributes ?? Array.Empty<string>();
        Badges = badges;
        Title = title;
        Description = description;
        CategoryId = categoryId;
        SellerId = sellerId;
        Photos = photos;
        this.Characteristics = characteristics;
    }
}