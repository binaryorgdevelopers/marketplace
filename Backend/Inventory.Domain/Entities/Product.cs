using System.Text.Json.Serialization;
using Inventory.Domain.Abstractions;
using Inventory.Domain.Extensions;

namespace Inventory.Domain.Entities;

public class Product : IIdentifiable, ICommon
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now.SetKindUtc();
    public DateTime UpdatedAt { get; set; } = DateTime.Now.SetKindUtc();
    public DateTime LastSession { get; set; } = DateTime.Now.SetKindUtc();

    public string[]? Attributes { get; set; }
    public string? Synonyms { get; set; }
    public string Title { get; set; }

    public decimal Price { get; set; }
    public int Count { get; set; }

    public string Description { get; set; }
    public Category Category { get; set; }
    public Seller Seller { get; set; }
    [JsonIgnore] public ICollection<Badge> Badges { get; set; }
    [JsonIgnore] public ICollection<Blob> Photos { get; set; }
    [JsonIgnore] public ICollection<Characteristics> Characteristics { get; set; }

    public Guid SellerId { get; set; }
    public Guid CategoryId { get; set; }

    public Product()
    {
    }

    public Product(IEnumerable<Badge> badges, string title,
        string description, Guid categoryId, Guid sellerId, IEnumerable<Blob> photos,
        IEnumerable<Characteristics> characteristics, Seller seller, Category category)
    {
        Id = Guid.NewGuid();
        Badges = badges.ToList();
        Title = title;
        Description = description;
        CategoryId = categoryId;
        SellerId = sellerId;
        Photos = photos.ToList();
        Characteristics = characteristics.ToList();
        Seller = seller;
        Category = category;
    }

    private Product(
        IEnumerable<Badge> badges,
        string title,
        decimal price,
        int count,
        string description,
        Guid categoryId,
        Guid sellerId,
        IEnumerable<Blob> photos, IEnumerable<Characteristics> characteristics)
    {
        Badges = badges.ToList();
        Title = title;
        Price = price;
        Count = count;
        Description = description;
        CategoryId = categoryId;
        SellerId = sellerId;
        Photos = photos.ToList();
        Characteristics = characteristics.ToList();
    }

    public static Product Create(
        string title,
        decimal price,
        int count,
        string description,
        Guid categoryId,
        Guid sellerId,
        IEnumerable<Blob> photos,
        IEnumerable<Characteristics> characteristics,
        IEnumerable<Badge> badges)
        => new(badges, title, price, count, description, categoryId, sellerId, photos, characteristics);
}