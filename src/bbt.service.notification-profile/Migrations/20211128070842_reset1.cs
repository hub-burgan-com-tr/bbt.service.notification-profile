using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class reset1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-EFT");

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-FAST");

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-QR");
        }
    }
}
