using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Database.EntityMaps;

internal class BlobMap : IEntityTypeConfiguration<Blob>
{
    public void Configure(EntityTypeBuilder<Blob> builder)
    {
        builder.ToTable(nameof(Blob));

        builder
            .HasOne(c => c.User)
            .WithMany(c => c.Files)
            .HasForeignKey(c => c.UserId);

        builder
            .HasOne(c => c.Shop)
            .WithMany(c => c.Files)
            .HasForeignKey(c => c.ShopId);

        builder
            .HasOne<Product>(c => c.Product)
            .WithMany(c => c.Photos)
            .HasForeignKey(c => c.ProductId);
    }
}