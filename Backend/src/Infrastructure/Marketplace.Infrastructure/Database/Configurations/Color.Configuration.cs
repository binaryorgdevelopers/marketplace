using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Database.Configurations;

public static class ColorConfiguration
{
    public static void ConfigureColor(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Color>(builder =>
        {
            builder.ToTable("Color");

            builder
                .HasOne<Characteristics>(c => c.Characteristics)
                .WithMany(c => c.Values)
                .HasForeignKey(c => c.Id);
        });
    }
}