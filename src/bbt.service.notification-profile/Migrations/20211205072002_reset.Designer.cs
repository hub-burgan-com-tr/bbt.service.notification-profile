﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Notification.Profile.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20211205072002_reset")]
    partial class reset
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Consumer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("$id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("$id"), 1L, 1);

                    b.Property<long>("Client")
                        .HasColumnType("bigint");

                    b.Property<string>("DeviceKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Filter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPushEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSmsEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<long>("User")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("$id")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("$id"));

                    b.HasIndex("SourceId");

                    b.ToTable("Consumers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1e15d57c-26e3-4e78-94f9-8649b3302555"),
                            Client = 123456L,
                            Filter = "Message.data.amount >= 500 && Message.data.iban ==\"TR1234567\"",
                            IsEmailEnabled = false,
                            IsPushEnabled = false,
                            IsSmsEnabled = true,
                            SourceId = 1,
                            User = 123456L
                        },
                        new
                        {
                            Id = new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"),
                            Client = 123456L,
                            IsEmailEnabled = false,
                            IsPushEnabled = false,
                            IsSmsEnabled = true,
                            SourceId = 102,
                            User = 123456L
                        },
                        new
                        {
                            Id = new Guid("3e15d57c-26e3-4e78-94f9-8649b3302555"),
                            Client = 0L,
                            Filter = "Message.data.amount >= 500000",
                            IsEmailEnabled = false,
                            IsPushEnabled = false,
                            IsSmsEnabled = true,
                            SourceId = 1,
                            User = 123456L
                        });
                });

            modelBuilder.Entity("Source", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ApiKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DisplayType")
                        .HasColumnType("int");

                    b.Property<string>("EmailServiceReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("PushServiceReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Secret")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SmsServiceReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title_EN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title_TR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Topic")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Sources");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ApiKey = "a1b2c33d4e5f6g7h8i9jakblc",
                            DisplayType = 4,
                            EmailServiceReference = "notify_email_incoming_eft",
                            PushServiceReference = "notify_push_incoming_eft",
                            Secret = "11561681-8ba5-4b46-bed0-905ae1769bc6",
                            SmsServiceReference = "notify_sms_incoming_eft",
                            Title_EN = "Incoming EFT",
                            Title_TR = "Gelen EFT",
                            Topic = "http://localhost:8082/topics/cdc_eft/incoming_eft"
                        },
                        new
                        {
                            Id = 101,
                            ApiKey = "a1b2c33d4e5f6g7h8i9jakblc",
                            DisplayType = 3,
                            EmailServiceReference = "notify_email_incoming_fast",
                            ParentId = 1,
                            PushServiceReference = "notify_push_incoming_fast",
                            Secret = "11561681-8ba5-4b46-bed0-905ae1769bc6",
                            SmsServiceReference = "notify_sms_incoming_fast",
                            Title_EN = "Incoming FAST",
                            Title_TR = "Gelen FAST",
                            Topic = "http://localhost:8082/topics/cdc_eft/incoming_fast"
                        },
                        new
                        {
                            Id = 10101,
                            ApiKey = "a1b2c33d4e5f6g7h8i9jakblc",
                            DisplayType = 1,
                            EmailServiceReference = "notify_email_incoming_fast",
                            ParentId = 101,
                            PushServiceReference = "notify_push_incoming_fast",
                            Secret = "11561681-8ba5-4b46-bed0-905ae1769bc6",
                            SmsServiceReference = "notify_sms_incoming_fast",
                            Title_EN = "Not Delivered FAST Messages",
                            Title_TR = "Ulasmayan FAST",
                            Topic = "http://localhost:8082/topics/cdc_eft/incoming_fast_not_delivered"
                        },
                        new
                        {
                            Id = 102,
                            ApiKey = "a1b2c33d4e5f6g7h8i9jakblc",
                            DisplayType = 3,
                            EmailServiceReference = "notify_email_incoming_qr",
                            ParentId = 1,
                            PushServiceReference = "notify_push_incoming_qr",
                            Secret = "11561681-8ba5-4b46-bed0-905ae1769bc6",
                            SmsServiceReference = "notify_sms_incoming_qr",
                            Title_EN = "Incoming QR",
                            Title_TR = "Gelen QR",
                            Topic = "http://localhost:8082/topics/cdc_eft/incoming_qr"
                        });
                });

            modelBuilder.Entity("SourceParameter", b =>
                {
                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<string>("JsonPath")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Title_EN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title_TR")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SourceId", "JsonPath", "Type");

                    b.ToTable("SourceParameter");

                    b.HasData(
                        new
                        {
                            SourceId = 101,
                            JsonPath = "Message.data.amount",
                            Type = 4,
                            Title_EN = "Amount",
                            Title_TR = "Tutar"
                        },
                        new
                        {
                            SourceId = 101,
                            JsonPath = "Message.data.amount",
                            Type = 3,
                            Title_EN = "Amount",
                            Title_TR = "Tutar"
                        },
                        new
                        {
                            SourceId = 1,
                            JsonPath = "Message.data.amount",
                            Type = 4,
                            Title_EN = "Amount",
                            Title_TR = "Tutar"
                        },
                        new
                        {
                            SourceId = 102,
                            JsonPath = "Message.data.amount",
                            Type = 4,
                            Title_EN = "Amount",
                            Title_TR = "Tutar"
                        });
                });

            modelBuilder.Entity("Consumer", b =>
                {
                    b.HasOne("Source", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Phone", "Phone", b1 =>
                        {
                            b1.Property<Guid>("ConsumerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("CountryCode")
                                .HasColumnType("int");

                            b1.Property<int>("Number")
                                .HasColumnType("int");

                            b1.Property<int>("Prefix")
                                .HasColumnType("int");

                            b1.HasKey("ConsumerId");

                            b1.ToTable("Consumers");

                            b1.WithOwner()
                                .HasForeignKey("ConsumerId");

                            b1.HasData(
                                new
                                {
                                    ConsumerId = new Guid("1e15d57c-26e3-4e78-94f9-8649b3302555"),
                                    CountryCode = 90,
                                    Number = 3855206,
                                    Prefix = 530
                                },
                                new
                                {
                                    ConsumerId = new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"),
                                    CountryCode = 90,
                                    Number = 3855206,
                                    Prefix = 530
                                },
                                new
                                {
                                    ConsumerId = new Guid("3e15d57c-26e3-4e78-94f9-8649b3302555"),
                                    CountryCode = 90,
                                    Number = 3855206,
                                    Prefix = 530
                                });
                        });

                    b.Navigation("Phone");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("Source", b =>
                {
                    b.HasOne("Source", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("SourceParameter", b =>
                {
                    b.HasOne("Source", "Source")
                        .WithMany("Parameters")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Source");
                });

            modelBuilder.Entity("Source", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("Parameters");
                });
#pragma warning restore 612, 618
        }
    }
}
