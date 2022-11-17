using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class AddProdTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Consumers",
                keyColumn: "Id",
                keyValue: new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"));

            migrationBuilder.DeleteData(
                table: "Consumers",
                keyColumn: "Id",
                keyValue: new Guid("3e15d57c-26e3-4e78-94f9-8649b3302555"));

            migrationBuilder.DeleteData(
                table: "SourceServices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SourceServices",
                columns: new[] { "Id", "ServiceUrl", "SourceId" },
                values: new object[] { 1, "X", 1 });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "ApiKey", "ClientIdJsonPath", "DisplayType", "EmailServiceReference", "KafkaCertificate", "KafkaDataTime", "KafkaUrl", "ParentId", "PushServiceReference", "Secret", "SmsServiceReference", "Title_EN", "Title_TR", "Topic" },
                values: new object[] { 1, "", null, 1, "notify_email_incoming_eft", "x", 0, "x", null, "notify_push_incoming_eft", "", "9cab7fdc-76a4-44be-b6fa-101f13729875", "CashBackEN", "CashBackTR", "CAMPAIGN_CASHBACK_ACCOUNTING_INFO" });

            migrationBuilder.InsertData(
                table: "Consumers",
                columns: new[] { "Id", "Client", "DefinitionCode", "DeviceKey", "Email", "Filter", "IsEmailEnabled", "IsPushEnabled", "IsSmsEnabled", "SourceId", "User", "Phone_CountryCode", "Phone_Number", "Phone_Prefix" },
                values: new object[] { new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"), 0L, null, null, null, null, false, false, true, 1, 0L, 90, 3855206, 530 });

            migrationBuilder.InsertData(
                table: "Consumers",
                columns: new[] { "Id", "Client", "DefinitionCode", "DeviceKey", "Email", "Filter", "IsEmailEnabled", "IsPushEnabled", "IsSmsEnabled", "SourceId", "User", "Phone_CountryCode", "Phone_Number", "Phone_Prefix" },
                values: new object[] { new Guid("3e15d57c-26e3-4e78-94f9-8649b3302555"), 0L, null, null, null, "", false, false, true, 1, 0L, 90, 3855206, 530 });
        }
    }
}
