using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Database;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DataContext()
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Blob> Blobs { get; set; }
    public DbSet<Shop> Shops { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("Users").HasKey(u => u.Id);
            builder
                .HasMany(x => x.Shops)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder
                .HasMany(c => c.Files)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
        });
        
        modelBuilder.Entity<Shop>(builder =>
        {
            builder.ToTable("Shops").HasKey(c => c.Id);
            builder
                .HasOne(c => c.User)
                .WithMany(c => c.Shops)
                .HasForeignKey(c => c.UserId);

            builder
                .HasMany(c => c.Files)
                .WithOne(c => c.Shop)
                .HasForeignKey(c => c.ShopId);
        });

        modelBuilder.Entity<Blob>(builder =>
        {
            builder.ToTable("Blob").HasKey(c => c.Id);

            builder
                .HasOne(c => c.User)
                .WithMany(c => c.Files);

            builder
                .HasOne(c => c.Shop)
                .WithMany(c => c.Files);
        });

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: User, State: EntityState.Modified });
        foreach (var entity in entries)
        {
            ((User)entity.Entity).UpdatedAt = DateTime.Now.SetKindUtc();
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: User, State: EntityState.Modified });
        foreach (var entity in entries)
        {
            ((User)entity.Entity).UpdatedAt = DateTime.Now.SetKindUtc();
        }

        return base.SaveChanges();
    }
}