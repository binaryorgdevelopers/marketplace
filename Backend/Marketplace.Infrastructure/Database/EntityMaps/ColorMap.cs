using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Database.EntityMaps;

internal class ColorMap : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.ToTable(nameof(Color));

        builder
            .HasOne<Characteristics>(c => c.Characteristics)
            .WithMany(c => c.Values)
            .HasForeignKey(c => c.Id);
    }
}