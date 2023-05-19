namespace Shared.BaseEntity;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastUsed { get; set; }
    public bool IsActive { get; set; }
}