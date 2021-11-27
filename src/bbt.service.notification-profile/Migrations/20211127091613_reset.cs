using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class reset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Topic = table.Column<string>(type: "TEXT", nullable: true),
                    ApiKey = table.Column<string>(type: "TEXT", nullable: true),
                    Secret = table.Column<string>(type: "TEXT", nullable: true),
                    PushServiceReference = table.Column<string>(type: "TEXT", nullable: true),
                    SmsServiceReference = table.Column<string>(type: "TEXT", nullable: true),
                    EmailServiceReference = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consumers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SourceId = table.Column<string>(type: "TEXT", nullable: true),
                    Client = table.Column<long>(type: "INTEGER", nullable: false),
                    User = table.Column<long>(type: "INTEGER", nullable: false),
                    Filter = table.Column<string>(type: "TEXT", nullable: true),
                    IsPushEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeviceKey = table.Column<string>(type: "TEXT", nullable: true),
                    IsSmsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Phone_CountryCode = table.Column<int>(type: "INTEGER", nullable: true),
                    Phone_Prefix = table.Column<int>(type: "INTEGER", nullable: true),
                    Phone_Number = table.Column<int>(type: "INTEGER", nullable: true),
                    IsMailEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consumers_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConsumerVariant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Key = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumerVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumerVariant_Consumers_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "Consumers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_SourceId",
                table: "Consumers",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumerVariant_ConsumerId",
                table: "ConsumerVariant",
                column: "ConsumerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumerVariant");

            migrationBuilder.DropTable(
                name: "Consumers");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
