using Marketplace.Application.Common.Extensions;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Database.Configurations;
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
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public DbSet<Color> Colors { get; set; }
    public DbSet<Blob> Blobs { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Characteristics> Characteristics { get; set; }
    public DbSet<Clients> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UserConfigurations.ConfigureUser(modelBuilder);
        BlobConfiguration.ConfigureBlob(modelBuilder);
        ProductConfiguration.ConfigureProduct(modelBuilder);
        CharacteristicsConfiguration.ConfigureCharacteristics(modelBuilder);
        ColorConfiguration.ConfigureColor(modelBuilder);
        CustomerConfiguration.ConfigureCustomer(modelBuilder);

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