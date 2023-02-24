using Marketplace.Domain.Entities;
using Marketplace.Domain.Models.Constants;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Database.Configurations;

public static class UserConfigurations
{
    public static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(c =>
            c.Property(c => c.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (Roles)Enum.Parse(typeof(Roles), v)));
        modelBuilder.Entity<User>(builder =>
        {
            builder
                .HasMany(c => c.Files)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
        });
        modelBuilder.Entity<Clients>(builder =>
        {
            builder.Property(c => c.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (Roles)Enum.Parse(typeof(Roles), v));
        });
    }
}