namespace Marketplace.Domain.Abstractions;

public interface ICommon
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }
}