using Authentication;
using Inventory.Domain.Abstractions;
using Shared.Extensions;
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
        Id = Guid.NewGuid();
        Title = title;
        Extras = extras;

        CreatedAt = DateTime.Now.SetKindUtc();
        UpdatedAt = DateTime.Now.SetKindUtc();
        LastSession = DateTime.Now.SetKindUtc();
    }

    private Blob(string title, string? extras, Guid productId)
    {
        Title = title;
        Extras = extras;
        ProductId = productId;
    }


    public Guid Id { get; set; }

    public string Title { get; set; }

    public string? Extras { get; set; }

    //FK s
    public Guid UserId { get; set; }
    public Product Product { get; set; }

    public Guid ProductId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }


    public static Blob Create(string title, string? extras)
    {
        return new Blob(title, extras);
    }

    public static Blob Create(string title, string? extras, Guid productId)
    {
        return new Blob(title, extras, productId);
    }
}