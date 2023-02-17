using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Marketplace.Infrastructure.Database;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Blob>Blobs { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Role> Roles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(c =>
        {
            c
                .HasMany(x => x.Shops)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        });
        base.OnModelCreating(modelBuilder);
    }

    public DataContext()
    {
        
    }
}