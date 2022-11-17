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
    [Migration("20220720062637_NotificationLogTableRemoveColumn")]
    partial class NotificationLogTableRemoveColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
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

                    b.Property<string>("DefinitionCode")
                        .HasColumnType("nvarchar(max)");

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
                            Id = new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"),
                            Client = 0L,
                            IsEmailEnabled = false,
                            IsPushEnabled = false,
                            IsSmsEnabled = true,
                            SourceId = 1,
                            User = 0L
                        },
                        new
                        {
                            Id = new Guid("3e15d57c-26e3-4e78-94f9-8649b3302555"),
                            Client = 0L,
                            Filter = "",
                            IsEmailEnabled = false,
                            IsPushEnabled = false,
                            IsSmsEnabled = true,
                            SourceId = 1,
                            User = 0L
                        });
                });

            modelBuilder.Entity("Notification.Profile.Model.Database.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Notification.Profile.Model.Database.LogDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LogId")
                        .HasColumnType("int");

                    b.Property<string>("RequestData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResponseData")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LogDetails");
                });

            modelBuilder.Entity("NotificationLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<long>("Client")
                        .HasColumnType("bigint");

                    b.Property<string>("Filter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPushEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSmsEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<long>("User")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("NotificationLogs", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("ReminderDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("DefinitionCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ReminderDefinitions");
                });

            modelBuilder.Entity("Source", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ApiKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientIdJsonPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DisplayType")
                        .HasColumnType("int");

                    b.Property<string>("EmailServiceReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KafkaUrl")
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
                            ApiKey = "",
                            DisplayType = 1,
                            EmailServiceReference = "notify_email_incoming_eft",
                            KafkaUrl = "x",
                            PushServiceReference = "notify_push_incoming_eft",
                            Secret = "",
                            SmsServiceReference = "9cab7fdc-76a4-44be-b6fa-101f13729875",
                            Title_EN = "CashBackEN",
                            Title_TR = "CashBackTR",
                            Topic = "CAMPAIGN_CASHBACK_ACCOUNTING_INFO"
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
                });

            modelBuilder.Entity("SourceService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ServiceUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SourceServices");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ServiceUrl = "X",
                            SourceId = 1
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
