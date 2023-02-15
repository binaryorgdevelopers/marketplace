using Marketplace.Domain.Abstractions;

namespace Marketplace.Domain.Entities;

public class Blob : IIdentifiable, ICommon
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string FileName { get; set; }
    public string Extras { get; set; }
}