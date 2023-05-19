using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence;

internal class IdentityContext : DbContext
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    public DbSet<User?> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public IdentityContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder
                .HasOne(c => c.Role)
                .WithOne(c => c.User);
        });
        modelBuilder.Entity<Role>(builder =>
        {
            builder.ToTable("roles");
            builder
                .HasOne(c => c.User)
                .WithOne(u => u.Role);
        });
        base.OnModelCreating(modelBuilder);
    }
}