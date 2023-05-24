﻿using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;

namespace Identity.Infrastructure.Persistence;

internal class IdentityContext : DbContext
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    public IdentityContext()
    {
    }

    public DbSet<User?> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder
                .HasOne(c => c.Role)
                .WithMany(c => c.User)
                .HasForeignKey(x=>x.RoleId);
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

        base.OnModelCreating(modelBuilder);
    }
}