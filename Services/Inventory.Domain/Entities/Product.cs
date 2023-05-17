using System.Text.Json.Serialization;
using Inventory.Domain.Abstractions;
using Inventory.Domain.Exceptions;
using Inventory.Domain.Extensions;

namespace Inventory.Domain.Entities;

/// <summary>
/// Represents Domain Entity for Products table 
/// </summary>
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

    /// <summary>
    /// Empty constructor for EF core 
    /// </summary>
    public Product()
    {
    }

    private Product(IEnumerable<Badge> badges, string title, decimal price, int count,
        string description, Guid categoryId, Guid sellerId, IEnumerable<Blob> photos,
        IEnumerable<Characteristics> characteristics)
    {
        NullException.ThrowIfNull(title);
        NullException.ThrowIfNull(price);
        NullException.ThrowIfNull(count);
        NullException.ThrowIfNull(description);

        Title = title;
        Price = price;
        Count = count;
        Description = description;

        CategoryId = categoryId;
        SellerId = sellerId;

        Badges = badges.ToList();
        Photos = photos.ToList();
        Characteristics = characteristics.ToList();
    }

    /// <summary>
    /// Factory method for creating new Product
    /// </summary>
    /// <param name="title"></param>
    /// <param name="price"></param>
    /// <param name="count"></param>
    /// <param name="description"></param>
    /// <param name="categoryId"></param>
    /// <param name="sellerId"></param>
    /// <param name="photos"></param>
    /// <param name="characteristics"></param>
    /// <param name="badges"></param>
    /// <returns></returns>
    public static Product Create(string title, decimal price, int count, string description, Guid categoryId,
        Guid sellerId, IEnumerable<Blob> photos, IEnumerable<Characteristics> characteristics,
        IEnumerable<Badge> badges)
        => new(badges, title, price, count, description, categoryId, sellerId, photos, characteristics);

    /// <summary>
    /// Method for decrementing count of product, whenever product sold
    /// </summary>
    /// <param name="count"></param>
    public void Decrement(int count = 1) => Count -= count;
}