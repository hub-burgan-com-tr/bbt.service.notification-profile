using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class sourcetableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Kafka",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DisplayType", "Kafka" },
                values: new object[] { 5, "test" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "DisplayType", "Kafka" },
                values: new object[] { 4, "test" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "DisplayType", "Kafka" },
                values: new object[] { 4, "test" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 10101,
                columns: new[] { "DisplayType", "Kafka" },
                values: new object[] { 2, "test" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kafka",
                table: "Sources");

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1,
                column: "DisplayType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 101,
                column: "DisplayType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 102,
                column: "DisplayType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 10101,
                column: "DisplayType",
                value: 1);
        }
    }
}
