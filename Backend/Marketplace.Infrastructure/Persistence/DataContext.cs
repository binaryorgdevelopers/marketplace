using Marketplace.Domain.Entities;
using Marketplace.Domain.Extensions;
using Marketplace.Infrastructure.Persistence.EntityMaps;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);


        foreach (var entry in entries)
        {
            var createdAt = entry.Entity.GetType().GetProperty("CreatedAt");
            if (createdAt is not null && createdAt.PropertyType == typeof(DateTime))
            {
                var createdAtValue = (DateTime)(createdAt.GetValue(entry.Entity) ?? DateTime.Now);
                createdAt.SetValue(entry.Entity, createdAtValue.SetKindUtc());
            }
            // ((User)entry.Entity).UpdatedAt = DateTime.Now.SetKindUtc();
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