using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class reset4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "SourceParameter");

            migrationBuilder.UpdateData(
                table: "Consumers",
                keyColumn: "Id",
                keyValue: new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"),
                column: "SourceId",
                value: "[SAMPLE]Incoming-QR");

            migrationBuilder.InsertData(
                table: "SourceParameter",
                columns: new[] { "JsonPath", "SourceId", "AutoGenerate", "Title_EN", "Title_TR", "Type" },
                values: new object[,]
                {
                    { "Message.data.amount", "[SAMPLE]Incoming-EFT", true, "Amount", "Tutar", 1 },
                    { "Message.data.amount", "[SAMPLE]Incoming-FAST", true, "Amount", "Tutar", 1 },
                    { "Message.data.amount", "[SAMPLE]Incoming-QR", true, "Amount", "Tutar", 1 }
                });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-EFT",
                columns: new[] { "Title_EN", "Title_TR" },
                values: new object[] { "Incoming EFT", "Gelen EFT" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-FAST",
                columns: new[] { "Title_EN", "Title_TR" },
                values: new object[] { "Incoming FAST", "Gelen FAST" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-QR",
                columns: new[] { "Title_EN", "Title_TR" },
                values: new object[] { "Incoming QR", "Gelen QR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SourceParameter",
                keyColumns: new[] { "JsonPath", "SourceId" },
                keyValues: new object[] { "Message.data.amount", "[SAMPLE]Incoming-EFT" });

            migrationBuilder.DeleteData(
                table: "SourceParameter",
                keyColumns: new[] { "JsonPath", "SourceId" },
                keyValues: new object[] { "Message.data.amount", "[SAMPLE]Incoming-FAST" });

            migrationBuilder.DeleteData(
                table: "SourceParameter",
                keyColumns: new[] { "JsonPath", "SourceId" },
                keyValues: new object[] { "Message.data.amount", "[SAMPLE]Incoming-QR" });

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "SourceParameter",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Consumers",
                keyColumn: "Id",
                keyValue: new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"),
                column: "SourceId",
                value: "[SAMPLE]Incoming-EFT");

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-EFT",
                columns: new[] { "Title_EN", "Title_TR" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-FAST",
                columns: new[] { "Title_EN", "Title_TR" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-QR",
                columns: new[] { "Title_EN", "Title_TR" },
                values: new object[] { null, null });
        }
    }
}
