using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Exceptions;

namespace Marketplace.Domain.Entities;

public class Blob : IIdentifiable, ICommon
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string FileName { get; private set; }
    public string Extras { get; private set; }

    public Blob(Guid id, string fileName, string extras)
    {
        Id = id;
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new AuthException(Codes.InvalidCredential, $"Field can't be empty: '{nameof(fileName)}'");
        }

        FileName = fileName;
        Extras = extras;
    }
}