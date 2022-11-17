using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class AddProductCodeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductCodeId",
                table: "Sources",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "MessageNotificationLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "MessageNotificationLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "MessageNotificationLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "MessageNotificationLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadTime",
                table: "MessageNotificationLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCodeId",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "MessageNotificationLogs");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "MessageNotificationLogs");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "MessageNotificationLogs");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "MessageNotificationLogs");

            migrationBuilder.DropColumn(
                name: "ReadTime",
                table: "MessageNotificationLogs");
        }
    }
}
