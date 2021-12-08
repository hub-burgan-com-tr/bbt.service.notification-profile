using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class clientidpath2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientIdJsonPath",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientIdJsonPath",
                table: "Sources");
        }
    }
}
