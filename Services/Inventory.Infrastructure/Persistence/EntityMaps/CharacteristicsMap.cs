using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Persistence.EntityMaps;

internal class CharacteristicsMap : IEntityTypeConfiguration<Characteristics>
{
    public void Configure(EntityTypeBuilder<Characteristics> builder)
    {
        builder.ToTable(nameof(Characteristics));

        builder
            .HasMany<Color>(c => c.Values)
            .WithOne(c => c.Characteristics)
            .HasForeignKey(c => c.CharId);

        builder
            .HasOne<Product>(c => c.Product)
            .WithMany(c => c.Characteristics)
            .HasForeignKey(c => c.ProductId);
    }
}