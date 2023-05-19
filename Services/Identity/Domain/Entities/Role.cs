using Shared.BaseEntity;

namespace Identity.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }
}