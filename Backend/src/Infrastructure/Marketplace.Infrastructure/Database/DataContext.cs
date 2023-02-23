using Marketplace.Application.Common.Extensions;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Converters;
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
    public DbSet<Seller>Sellers { get; set; }
    public DbSet<Customer>Customers { get; set; }
    
    public DbSet<Color>Colors { get; set; }
    public DbSet<Blob> Blobs { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Characteristics> Characteristics { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
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

        modelBuilder.Entity<Product>();

        modelBuilder.Entity<Product>(builder =>
        {
            builder.Property(c => c.Attributes)
                .HasConversion(v => string.Join(",", v),
                    c => c.StringToArray());

            builder.Property(c => c.Badges)
                .HasConversion(v => string.Join(",", v),
                    c => c.StringToArray());

            builder.ToTable("Products").HasKey(p => p.Id);
            builder
                .HasMany<Blob>(c => c.Photos)
                .WithOne(c => c.Product)
                .HasForeignKey(c => c.ProductId);

            builder
                .HasMany<Characteristics>(c => c.Characteristics)
                .WithOne(c => c.Product)
                .HasForeignKey(c => c.ProductId);

            builder
                .HasOne<Seller>(c => c.Seller)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.SellerId);

            builder
                .HasOne<Category>(c => c.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.CategoryId);
        });

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

        modelBuilder.Entity<Color>(builder =>
        {
            builder.ToTable("Color");

            builder
                .HasOne<Characteristics>(c => c.Characteristics)
                .WithMany(c => c.Values)
                .HasForeignKey(c => c.Id);
        });

        modelBuilder
            .Entity<Customer>(builder =>
            {
                builder.Property(e => e.Authorities)
                    .HasConversion(v => string.Join(",", v),
                        c => c.StringToArray());
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