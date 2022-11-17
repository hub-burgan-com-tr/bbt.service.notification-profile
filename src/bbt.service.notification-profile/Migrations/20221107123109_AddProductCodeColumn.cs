using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class AddProductCodeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCode",
                table: "ProductCode");

            migrationBuilder.RenameTable(
                name: "ProductCode",
                newName: "ProductCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCodes",
                table: "ProductCodes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCodes",
                table: "ProductCodes");

            migrationBuilder.RenameTable(
                name: "ProductCodes",
                newName: "ProductCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCode",
                table: "ProductCode",
                column: "Id");
        }
    }
}
