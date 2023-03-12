using Marketplace.Domain.Abstractions;

namespace Marketplace.Domain.Entities;

public class Badge : IIdentifiable, ICommon
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }

    public string Text { get; set; }
    public string TextColor { get; set; }
    public string BackgroundColor { get; set; }
    public string Description { get; set; }
    public string? Link { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }


    public Badge(string text, string textColor, string backgroundColor, string description)
    {
        Id = Guid.NewGuid();
        Text = text;
        TextColor = textColor;
        BackgroundColor = backgroundColor;
        Description = description;
    }

    public Badge()
    {
        
    }
}