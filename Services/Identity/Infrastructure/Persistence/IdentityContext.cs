using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;

namespace Identity.Infrastructure.Persistence;

public class IdentityContext : DbContext
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    public IdentityContext()
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<CardDetail> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder
                .HasOne(c => c.Role)
                .WithMany(c => c.User)
                .HasForeignKey(x => x.RoleId);
            // .WithOne(c => c.User);
            builder
                .Property(e => e.Authorities)
                .HasConversion(v => string.Join(",", v),
                    c => c.ToStringList());
        });
        modelBuilder.Entity<Role>(builder =>
        {
            builder.ToTable("roles");
            builder
                .HasMany(c => c.User).WithOne(c => c.Role);
        });
        modelBuilder.Entity<CardDetail>(builder =>
        {
            builder.ToTable("cards");
            builder.HasOne(c => c.User)
                .WithMany(c => c.Cards)
                .HasForeignKey(c => c.UserId);
        });

        base.OnModelCreating(modelBuilder);
    }
}