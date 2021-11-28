using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class reset2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConsumerVariants",
                columns: new[] { "Id", "ConsumerId", "Key", "Value" },
                values: new object[] { new Guid("c5f3cc77-debf-40a5-9371-98f397ee8969"), new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"), "IBAN", "TR58552069008" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConsumerVariants",
                keyColumn: "Id",
                keyValue: new Guid("c5f3cc77-debf-40a5-9371-98f397ee8969"));
        }
    }
}
