using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregatesModel.BuyerAggregate;

namespace Ordering.Infrastructure.EntityConfiguration;

internal class CardTypeEntityTypeConfiguration : IEntityTypeConfiguration<CardType>
{
    public void Configure(EntityTypeBuilder<CardType> cardTypesConfiguration)
    {
        cardTypesConfiguration.ToTable("cardtypes", OrderingContext.DEFAULT_SCHEMA);

        cardTypesConfiguration.HasKey(ct => ct.Id);

        cardTypesConfiguration.Property(ct => ct.Id)
            .HasDefaultValue(Guid.NewGuid())
            .ValueGeneratedNever()
            .IsRequired();

        cardTypesConfiguration.Property(ct => ct.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}