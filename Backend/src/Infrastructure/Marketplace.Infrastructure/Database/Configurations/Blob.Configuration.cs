using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Database.Configurations;

public static class BlobConfiguration
{
    public static void ConfigureBlob(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blob>(builder =>
        {
            builder.ToTable("Blob").HasKey(c => c.Id);

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
        });
    }
}