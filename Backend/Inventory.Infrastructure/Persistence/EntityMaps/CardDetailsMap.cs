using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Persistence.EntityMaps;

public class CardDetailsMap : IEntityTypeConfiguration<CardDetail>
{
    public void Configure(EntityTypeBuilder<CardDetail> builder)
    {
        builder.ToTable("CardDetails");

        builder
            .HasOne(c => c.Customer)
            .WithMany(c => c.CardDetails);

        // builder.Ignore(e => e.CardNumber);
        // builder.Ignore(e => e.ExpirationMonth);
        // builder.Ignore(e => e.ExpirationYear);
        // builder.Ignore(e => e.CardHolderName);
        // builder.Ignore(e => e.CVV);
    }
}