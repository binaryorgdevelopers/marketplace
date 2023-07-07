using Identity.Models.Dtos;
using Shared.BaseEntity;
using Shared.Extensions;

namespace Identity.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }

    public List<User> User { get; set; }

    public Role(string name)
    {
        Name = name;
        CreatedAt = DateTime.Now.SetKindUtc();
        UpdatedAt = DateTime.Now.SetKindUtc();
        LastUsed = DateTime.Now.SetKindUtc();
        IsActive = true;
    }

    public RoleDto ToDto() => new(Id, Name);
}