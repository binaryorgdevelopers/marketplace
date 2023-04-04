using Marketplace.Application.Common.Extensions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Persistence.EntityMaps;

internal class CustomerMap : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(nameof(Customer));
        
        builder.Property(e => e.Authorities)
            .HasConversion(v => string.Join(",", v),
                c => c.StringToArray());
    }
}