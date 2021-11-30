using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class reset2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConsumerVariants",
                keyColumn: "Id",
                keyValue: new Guid("c1685b57-41d6-4f45-9593-5ec8075b8a20"));

            migrationBuilder.RenameColumn(
                name: "IsMailEnabled",
                table: "Consumers",
                newName: "IsEmailEnabled");

            migrationBuilder.InsertData(
                table: "ConsumerVariants",
                columns: new[] { "Id", "ConsumerId", "Key", "Value" },
                values: new object[] { new Guid("444fcb0a-d7a7-4891-9a95-826cbef5d792"), new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"), "IBAN", "TR58552069008" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConsumerVariants",
                keyColumn: "Id",
                keyValue: new Guid("444fcb0a-d7a7-4891-9a95-826cbef5d792"));

            migrationBuilder.RenameColumn(
                name: "IsEmailEnabled",
                table: "Consumers",
                newName: "IsMailEnabled");

            migrationBuilder.InsertData(
                table: "ConsumerVariants",
                columns: new[] { "Id", "ConsumerId", "Key", "Value" },
                values: new object[] { new Guid("c1685b57-41d6-4f45-9593-5ec8075b8a20"), new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"), "IBAN", "TR58552069008" });
        }
    }
}
