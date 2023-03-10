using Marketplace.Application.Common.Extensions;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Database.EntityMaps;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Database;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }


    public DbSet<User> Users => Set<User>();
    public DbSet<Seller> Sellers => Set<Seller>();
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Color> Colors => Set<Color>();
    public DbSet<Blob> Blobs => Set<Blob>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Characteristics> Characteristics => Set<Characteristics>();
    public DbSet<Clients> Clients => Set<Clients>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new BlobMap())
            .ApplyConfiguration(new CategoryMap())
            .ApplyConfiguration(new CharacteristicsMap())
            .ApplyConfiguration(new ColorMap())
            .ApplyConfiguration(new CustomerMap())
            .ApplyConfiguration(new ProductMap())
            .ApplyConfiguration(new UserMap());
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