using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class AddSourceTableSaveInbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SaveInbox",
                table: "Sources",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaveInbox",
                table: "Sources");
        }
    }
}
