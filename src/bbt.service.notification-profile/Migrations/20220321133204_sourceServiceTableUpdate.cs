using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class sourceServiceTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SourceServices",
                columns: new[] { "Id", "ServiceUrl", "SourceId" },
                values: new object[] { 2, "localhost:/get2customerId", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SourceServices",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
