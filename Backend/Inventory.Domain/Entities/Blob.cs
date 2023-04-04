using Inventory.Domain.Abstractions;
using Inventory.Domain.Exceptions;
using Inventory.Domain.Models.Constants;

namespace Inventory.Domain.Entities;

public class Blob : IIdentifiable, ICommon
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }
    public string Title { get; private set; }
    public string? Extras { get; private set; }


    //FK s
    public User? User { get; set; }
    public Product Product { get; set; }

    public Guid? UserId { get; set; }
    public Guid ProductId { get; set; }


    public Blob()
    {
    }

    private Blob(string title, string? extras)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new AuthException(Codes.InvalidCredential, $"Field can't be empty: '{nameof(title)}'");
        }

        Title = title;
        Extras = extras;
    }

    private Blob(string title, string? extras, Guid productId)
    {
        Title = title;
        Extras = extras;
        ProductId = productId;
    }


    public static Blob Create(string title, string? extras) =>
        new(title, extras);

    public static Blob Create(string title, string? extras, Guid productId) =>
        new(title, extras, productId);
}