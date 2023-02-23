using Marketplace.Domain.Abstractions;

namespace Marketplace.Domain.Entities;

public class Product : IIdentifiable, ICommon
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }

    public string[] Attributes { get; set; }
    public string[] Badges { get; set; }
    public string Synonims { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public Seller Seller { get; set; }
    public User CreatedBy { get; set; }
    public List<Blob> Photos { get; set; }
    public List<Characteristics> Characteristics { get; set; }

    public Guid SellerId { get; set; }
    public Guid CategoryId { get; set; }

    public Product()
    {
        
    }
}