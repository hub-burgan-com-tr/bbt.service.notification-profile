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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title_TR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    DisplayType = table.Column<int>(type: "int", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Secret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PushServiceReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmsServiceReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailServiceReference = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sources_Sources_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Consumers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    Client = table.Column<long>(type: "bigint", nullable: false),
                    User = table.Column<long>(type: "bigint", nullable: false),
                    Filter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPushEnabled = table.Column<bool>(type: "bit", nullable: false),
                    DeviceKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSmsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Phone_CountryCode = table.Column<int>(type: "int", nullable: true),
                    Phone_Prefix = table.Column<int>(type: "int", nullable: true),
                    Phone_Number = table.Column<int>(type: "int", nullable: true),
                    IsEmailEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id = table.Column<long>(name: "$id", type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumers", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Consumers_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SourceParameter",
                columns: table => new
                {
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    JsonPath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title_TR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title_EN = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceParameter", x => new { x.SourceId, x.JsonPath, x.Type });
                    table.ForeignKey(
                        name: "FK_SourceParameter_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "ApiKey", "DisplayType", "EmailServiceReference", "ParentId", "PushServiceReference", "Secret", "SmsServiceReference", "Title_EN", "Title_TR", "Topic" },
                values: new object[] { 1, "a1b2c33d4e5f6g7h8i9jakblc", 4, "notify_email_incoming_eft", null, "notify_push_incoming_eft", "11561681-8ba5-4b46-bed0-905ae1769bc6", "notify_sms_incoming_eft", "Incoming EFT", "Gelen EFT", "http://localhost:8082/topics/cdc_eft/incoming_eft" });

            migrationBuilder.InsertData(
                table: "Consumers",
                columns: new[] { "Id", "Client", "DeviceKey", "Email", "Filter", "IsEmailEnabled", "IsPushEnabled", "IsSmsEnabled", "SourceId", "User", "Phone_CountryCode", "Phone_Number", "Phone_Prefix" },
                values: new object[,]
                {
                    { new Guid("1e15d57c-26e3-4e78-94f9-8649b3302555"), 123456L, null, null, "Message.data.amount >= 500 && Message.data.iban ==\"TR1234567\"", false, false, true, 1, 123456L, 90, 3855206, 530 },
                    { new Guid("3e15d57c-26e3-4e78-94f9-8649b3302555"), 0L, null, null, "Message.data.amount >= 500000", false, false, true, 1, 123456L, 90, 3855206, 530 }
                });

            migrationBuilder.InsertData(
                table: "SourceParameter",
                columns: new[] { "JsonPath", "SourceId", "Type", "Title_EN", "Title_TR" },
                values: new object[] { "Message.data.amount", 1, 4, "Amount", "Tutar" });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "ApiKey", "DisplayType", "EmailServiceReference", "ParentId", "PushServiceReference", "Secret", "SmsServiceReference", "Title_EN", "Title_TR", "Topic" },
                values: new object[,]
                {
                    { 101, "a1b2c33d4e5f6g7h8i9jakblc", 3, "notify_email_incoming_fast", 1, "notify_push_incoming_fast", "11561681-8ba5-4b46-bed0-905ae1769bc6", "notify_sms_incoming_fast", "Incoming FAST", "Gelen FAST", "http://localhost:8082/topics/cdc_eft/incoming_fast" },
                    { 102, "a1b2c33d4e5f6g7h8i9jakblc", 3, "notify_email_incoming_qr", 1, "notify_push_incoming_qr", "11561681-8ba5-4b46-bed0-905ae1769bc6", "notify_sms_incoming_qr", "Incoming QR", "Gelen QR", "http://localhost:8082/topics/cdc_eft/incoming_qr" }
                });

            migrationBuilder.InsertData(
                table: "Consumers",
                columns: new[] { "Id", "Client", "DeviceKey", "Email", "Filter", "IsEmailEnabled", "IsPushEnabled", "IsSmsEnabled", "SourceId", "User", "Phone_CountryCode", "Phone_Number", "Phone_Prefix" },
                values: new object[] { new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"), 123456L, null, null, null, false, false, true, 102, 123456L, 90, 3855206, 530 });

            migrationBuilder.InsertData(
                table: "SourceParameter",
                columns: new[] { "JsonPath", "SourceId", "Type", "Title_EN", "Title_TR" },
                values: new object[,]
                {
                    { "Message.data.amount", 101, 3, "Amount", "Tutar" },
                    { "Message.data.amount", 101, 4, "Amount", "Tutar" },
                    { "Message.data.amount", 102, 4, "Amount", "Tutar" }
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "ApiKey", "DisplayType", "EmailServiceReference", "ParentId", "PushServiceReference", "Secret", "SmsServiceReference", "Title_EN", "Title_TR", "Topic" },
                values: new object[] { 10101, "a1b2c33d4e5f6g7h8i9jakblc", 1, "notify_email_incoming_fast", 101, "notify_push_incoming_fast", "11561681-8ba5-4b46-bed0-905ae1769bc6", "notify_sms_incoming_fast", "Not Delivered FAST Messages", "Ulasmayan FAST", "http://localhost:8082/topics/cdc_eft/incoming_fast_not_delivered" });

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_$id",
                table: "Consumers",
                column: "$id",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_SourceId",
                table: "Consumers",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_ParentId",
                table: "Sources",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consumers");

            migrationBuilder.DropTable(
                name: "SourceParameter");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
