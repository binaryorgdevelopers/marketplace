﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Ordering.Infrastructure;

#nullable disable

namespace Davr.Services.Marketplace.Ordering.Infrastructure.Migrations
{
    [DbContext(typeof(OrderingContext))]
    [Migration("20230428115923_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.BuyerAggregate.Buyer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("IdentityGuid")
                        .HasMaxLength(200)
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IdentityGuid")
                        .IsUnique();

                    b.ToTable("buyers", "ordering");
                });

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.BuyerAggregate.CardType", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("34a69b75-05a5-4ab7-abae-a2c9d7ca7018"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("cardtypes", "ordering");
                });

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.BuyerAggregate.PaymentMethod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BuyerId")
                        .HasColumnType("uuid");

                    b.Property<string>("_alias")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("Alias");

                    b.Property<string>("_cardHolderName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("CardHolderName");

                    b.Property<string>("_cardNumber")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)")
                        .HasColumnName("CardNumber");

                    b.Property<Guid>("_cardTypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("CardTypeId");

                    b.Property<DateTime>("_expiration")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("Expiration");

                    b.Property<string>("_securityNumber")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("_cardTypeId");

                    b.ToTable("paymentmethods", "ordering");
                });

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.OrderAggregate.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid?>("_buyerId")
                        .HasColumnType("uuid")
                        .HasColumnName("BuyerId");

                    b.Property<DateTime>("_orderDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("OrderDate");

                    b.Property<Guid>("_orderStatusId")
                        .HasColumnType("uuid")
                        .HasColumnName("OrderStatusId");

                    b.Property<Guid?>("_paymentMethodId")
                        .HasColumnType("uuid")
                        .HasColumnName("PaymentMethodId");

                    b.HasKey("Id");

                    b.HasIndex("_buyerId");

                    b.HasIndex("_orderStatusId");

                    b.HasIndex("_paymentMethodId");

                    b.ToTable("orders", "ordering");
                });

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.OrderAggregate.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("_discount")
                        .HasColumnType("numeric")
                        .HasColumnName("Discount");

                    b.Property<string>("_pictureUrl")
                        .HasColumnType("text")
                        .HasColumnName("PictureUrl");

                    b.Property<string>("_productName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ProductName");

                    b.Property<decimal>("_unitPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("UnitPrice");

                    b.Property<int>("_units")
                        .HasColumnType("integer")
                        .HasColumnName("Units");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("orderItems", "ordering");
                });

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.OrderAggregate.OrderStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("aa122726-a2da-421e-81f8-dcee1b5ccaee"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("orderstatus", "ordering");
                });

            modelBuilder.Entity("Ordering.Infrastructure.Idempotency.ClientRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("requests", "ordering");
                });

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.BuyerAggregate.PaymentMethod", b =>
                {
                    b.HasOne("Ordering.Domain.AggregatesModel.BuyerAggregate.Buyer", null)
                        .WithMany("PaymentMethods")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ordering.Domain.AggregatesModel.BuyerAggregate.CardType", "CardType")
                        .WithMany()
                        .HasForeignKey("_cardTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CardType");
                });

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.OrderAggregate.Order", b =>
                {
                    b.HasOne("Ordering.Domain.AggregatesModel.BuyerAggregate.Buyer", null)
                        .WithMany()
                        .HasForeignKey("_buyerId");

                    b.HasOne("Ordering.Domain.AggregatesModel.OrderAggregate.OrderStatus", "OrderStatus")
                        .WithMany()
                        .HasForeignKey("_orderStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ordering.Domain.AggregatesModel.BuyerAggregate.PaymentMethod", null)
                        .WithMany()
                        .HasForeignKey("_paymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("Ordering.Domain.AggregatesModel.OrderAggregate.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("OrderId");

                            b1.ToTable("orders", "ordering");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("OrderStatus");
                });

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.OrderAggregate.OrderItem", b =>
                {
                    b.HasOne("Ordering.Domain.AggregatesModel.OrderAggregate.Order", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.BuyerAggregate.Buyer", b =>
                {
                    b.Navigation("PaymentMethods");
                });

            modelBuilder.Entity("Ordering.Domain.AggregatesModel.OrderAggregate.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}