using Marketplace.Application.Common.Extensions;
using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Database.Configurations;

public static class ProductConfiguration
{
    public static void ConfigureProduct(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(builder =>
        {
            builder.ToTable("Products").HasKey(p => p.Id);
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
                .HasOne<Seller>(c => c.Seller)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.SellerId);

            builder
                .HasOne<Category>(c => c.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.CategoryId);

            builder
                .HasMany<Badge>(c => c.Badges)
                .WithOne(c => c.Product)
                .HasForeignKey(c => c.ProductId);
        });
    }
}