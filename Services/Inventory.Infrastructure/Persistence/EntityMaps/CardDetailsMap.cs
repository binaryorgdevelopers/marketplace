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

        // builder.Property(c => c.Cn)
        //     .HasConversion(
        //         set => CardDetail.Encrypt(set),
        //         get => CardDetail.Decrypt(get)
        //     );
        // builder.Property(c => c.Chn)
        //     .HasConversion(
        //         set => CardDetail.Encrypt(set),
        //         get => CardDetail.Decrypt(get)
        //     );
        // builder.Property(c => c.Ey)
        //     .HasConversion(
        //         set => CardDetail.Encrypt(set),
        //         get => CardDetail.Decrypt(get)
        //     );
        // builder.Property(c => c.Cv)
        //     .HasConversion(
        //         set => CardDetail.Encrypt(set),
        //         get => CardDetail.Decrypt(get)
        //     );
        // builder.Property(c => c.Em)
        //     .HasConversion(
        //         set => CardDetail.Encrypt(set),
        //         get => CardDetail.Decrypt(get)
        //     );
    }
}