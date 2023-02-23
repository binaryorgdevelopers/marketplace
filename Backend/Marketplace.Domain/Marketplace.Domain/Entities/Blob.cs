using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Exceptions;

namespace Marketplace.Domain.Entities;

public class Blob : IIdentifiable, ICommon
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }
    public string Title { get; private set; }
    public string Extras { get; private set; }
    
    
    //FK s
    public User User { get; set; }
    public Shop Shop { get; set; }
    public Product Product { get; set; }
    
    public Guid UserId { get; set; }
    public Guid ShopId { get; set; }
    public Guid ProductId { get; set; }


    public Blob()
    {
        
    }
    public Blob(Guid id, string title, string extras)
    {
        Id = id;
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new AuthException(Codes.InvalidCredential, $"Field can't be empty: '{nameof(title)}'");
        }

        Title = title;
        Extras = extras;
    }
}