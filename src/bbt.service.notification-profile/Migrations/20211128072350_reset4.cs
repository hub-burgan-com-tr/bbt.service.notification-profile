using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class reset4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "$id",
                table: "Consumers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_$id",
                table: "Consumers",
                column: "$id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_Id",
                table: "Consumers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Consumers_$id",
                table: "Consumers");

            migrationBuilder.DropIndex(
                name: "IX_Consumers_Id",
                table: "Consumers");

            migrationBuilder.DropColumn(
                name: "$id",
                table: "Consumers");
        }
    }
}
