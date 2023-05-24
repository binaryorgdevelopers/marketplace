using Authentication;
using Inventory.Domain.Abstractions;
using Shared.Models.Constants;

namespace Inventory.Domain.Entities;

public class Blob : IIdentifiable, ICommon
{
    public Blob()
    {
    }

    private Blob(string title, string? extras)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new AuthException(Codes.InvalidCredential, $"Field can't be empty: '{nameof(title)}'");

        Title = title;
        Extras = extras;
    }

    private Blob(string title, string? extras, Guid productId)
    {
        Title = title;
        Extras = extras;
        ProductId = productId;
    }

    public string Title { get; }
    public string? Extras { get; }


    //FK s
    public User? User { get; set; }
    public Product Product { get; set; }

    public Guid? UserId { get; set; }
    public Guid ProductId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }
    public Guid Id { get; set; }


    public static Blob Create(string title, string? extras)
    {
        return new(title, extras);
    }

    public static Blob Create(string title, string? extras, Guid productId)
    {
        return new(title, extras, productId);
    }
}