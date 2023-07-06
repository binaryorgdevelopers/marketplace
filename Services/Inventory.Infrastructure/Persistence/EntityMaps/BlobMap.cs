using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Persistence.EntityMaps;

internal class BlobMap : IEntityTypeConfiguration<Blob>
{
    public void Configure(EntityTypeBuilder<Blob> builder)
    {
        builder.ToTable(nameof(Blob));


        builder
            .HasOne<Product>(c => c.Product)
            .WithMany(c => c.Photos)
            .HasForeignKey(c => c.ProductId);
    }
}