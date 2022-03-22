using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class sourceServiceDataAddde : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SourceServices",
                columns: new[] { "Id", "ServiceUrl", "SourceId" },
                values: new object[] { 3, "localhost:/get3customerId", 101 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SourceServices",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
