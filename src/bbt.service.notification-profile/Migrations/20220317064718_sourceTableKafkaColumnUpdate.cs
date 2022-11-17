using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class sourceTableKafkaColumnUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Kafka",
                table: "Sources",
                newName: "KafkaUrl");

            migrationBuilder.UpdateData(
                table: "SourceServices",
                keyColumn: "Id",
                keyValue: 1,
                column: "ServiceUrl",
                value: "localhost:/getcustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KafkaUrl",
                table: "Sources",
                newName: "Kafka");

            migrationBuilder.UpdateData(
                table: "SourceServices",
                keyColumn: "Id",
                keyValue: 1,
                column: "ServiceUrl",
                value: "test");
        }
    }
}
