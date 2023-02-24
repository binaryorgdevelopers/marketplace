using Marketplace.Application.Common.Extensions;
using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Database.Configurations;

public static class CustomerConfiguration
{
    public static void ConfigureCustomer(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Customer>(builder =>
            {
                builder.Property(e => e.Authorities)
                    .HasConversion(v => string.Join(",", v),
                        c => c.StringToArray());
            });
    }
}