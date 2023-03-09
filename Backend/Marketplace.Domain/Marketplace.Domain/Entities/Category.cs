using System.Text.Json.Serialization;
using Marketplace.Domain.Abstractions;

namespace Marketplace.Domain.Entities;

public class Category : IIdentifiable, ICommon
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }

    public string Title { get; set; }
    public int ProductAmount { get; set; }

    public Guid? ParentId { get; set; }
    public Category Parent { get; set; }
    public ICollection<Category> Children { get; set; }

    [JsonIgnore] public IEnumerable<Product> Products { get; set; }

    public Category(string title)
    {
        Title = title;
    }

    public Category()
    {
    }

    public Category(string title, Category parent = null)
    {
        Title = title;
        Parent = parent;
    }
}