using Marketplace.Domain.Entities;
using Marketplace.Domain.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Database.EntityMaps;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(c => c.Role)
            .HasConversion(
                c => c.ToString(),
                v => (Roles)Enum.Parse(typeof(Roles), v));

        builder
            .HasMany(c => c.Files)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

        builder.Property(c => c.Role)
            .HasConversion(
                v => v.ToString(),
                v => (Roles)Enum.Parse(typeof(Roles), v));
    }
}