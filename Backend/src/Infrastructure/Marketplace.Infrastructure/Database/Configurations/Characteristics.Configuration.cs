using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Database.Configurations;

public static class CharacteristicsConfiguration
{
    public static void ConfigureCharacteristics(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Characteristics>(builder =>
        {
            builder.ToTable("Characteristics");

            builder
                .HasMany<Color>(c => c.Values)
                .WithOne(c => c.Characteristics)
                .HasForeignKey(c => c.CharId);

            builder
                .HasOne<Product>(c => c.Product)
                .WithMany(c => c.Characteristics)
                .HasForeignKey(c => c.ProductId);
        });
    }
}