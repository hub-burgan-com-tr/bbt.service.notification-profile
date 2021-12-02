using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class reset2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Sources",
                newName: "Title_TR");

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title_EN",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-EFT",
                column: "Title_TR",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-FAST",
                column: "Title_TR",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-QR",
                column: "Title_TR",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "Title_EN",
                table: "Sources");

            migrationBuilder.RenameColumn(
                name: "Title_TR",
                table: "Sources",
                newName: "Title");

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-EFT",
                column: "Title",
                value: "Gelen EFT");

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-FAST",
                column: "Title",
                value: "Gelen Fast");

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: "[SAMPLE]Incoming-QR",
                column: "Title",
                value: "Gelen EFT");
        }
    }
}
