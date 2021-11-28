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
                name: "ConsumerVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumerVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumerVariants_Consumers_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "Consumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "ApiKey", "EmailServiceReference", "PushServiceReference", "Secret", "SmsServiceReference", "Title", "Topic" },
                values: new object[] { "[SAMPLE]Incoming-EFT", "a1b2c33d4e5f6g7h8i9jakblc", "notify_email_incoming_eft", "notify_push_incoming_eft", "11561681-8ba5-4b46-bed0-905ae1769bc6", "notify_sms_incoming_eft", "Gelen EFT", "http://localhost:8082/topics/cdc_eft/incoming_eft" });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "ApiKey", "EmailServiceReference", "PushServiceReference", "Secret", "SmsServiceReference", "Title", "Topic" },
                values: new object[] { "[SAMPLE]Incoming-FAST", "a1b2c33d4e5f6g7h8i9jakblc", "notify_email_incoming_fast", "notify_push_incoming_fast", "11561681-8ba5-4b46-bed0-905ae1769bc6", "notify_sms_incoming_fast", "Gelen Fast", "http://localhost:8082/topics/cdc_eft/incoming_fast" });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "ApiKey", "EmailServiceReference", "PushServiceReference", "Secret", "SmsServiceReference", "Title", "Topic" },
                values: new object[] { "[SAMPLE]Incoming-QR", "a1b2c33d4e5f6g7h8i9jakblc", "notify_email_incoming_qr", "notify_push_incoming_qr", "11561681-8ba5-4b46-bed0-905ae1769bc6", "notify_sms_incoming_qr", "Gelen EFT", "http://localhost:8082/topics/cdc_eft/incoming_qr" });

            migrationBuilder.InsertData(
                table: "Consumers",
                columns: new[] { "Id", "Client", "DeviceKey", "Email", "Filter", "IsMailEnabled", "IsPushEnabled", "IsSmsEnabled", "SourceId", "User", "Phone_CountryCode", "Phone_Number", "Phone_Prefix" },
                values: new object[] { new Guid("1e15d57c-26e3-4e78-94f9-8649b3302555"), 123456L, null, null, "data.amount >= 500", false, false, true, "[SAMPLE]Incoming-EFT", 123456L, 90, 3855206, 530 });

            migrationBuilder.InsertData(
                table: "Consumers",
                columns: new[] { "Id", "Client", "DeviceKey", "Email", "Filter", "IsMailEnabled", "IsPushEnabled", "IsSmsEnabled", "SourceId", "User", "Phone_CountryCode", "Phone_Number", "Phone_Prefix" },
                values: new object[] { new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"), 123456L, null, null, null, false, false, true, "[SAMPLE]Incoming-EFT", 123456L, 90, 3855206, 530 });

            migrationBuilder.InsertData(
                table: "Consumers",
                columns: new[] { "Id", "Client", "DeviceKey", "Email", "Filter", "IsMailEnabled", "IsPushEnabled", "IsSmsEnabled", "SourceId", "User", "Phone_CountryCode", "Phone_Number", "Phone_Prefix" },
                values: new object[] { new Guid("3e15d57c-26e3-4e78-94f9-8649b3302555"), 0L, null, null, "data.amount >= 500000", false, false, true, "[SAMPLE]Incoming-EFT", 123456L, 90, 3855206, 530 });

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_Id",
                table: "Consumers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_SourceId",
                table: "Consumers",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumerVariants_ConsumerId",
                table: "ConsumerVariants",
                column: "ConsumerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumerVariants");

            migrationBuilder.DropTable(
                name: "Consumers");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
