using Inventory.Domain.Entities;
using Marketplace.Application.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Persistence.EntityMaps;

internal class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product)).HasKey(p => p.Id);
        builder.Property(c => c.Attributes)
            .HasConversion(v => string.Join(",", v),
                c => c.StringToArray());

        builder
            .HasMany<Blob>(c => c.Photos)
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId);

        builder
            .HasMany<Characteristics>(c => c.Characteristics)
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId);

        builder
            .HasOne<Category>(c => c.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(c => c.CategoryId);

        builder
            .HasMany<Badge>(c => c.Badges)
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId);
    }
}