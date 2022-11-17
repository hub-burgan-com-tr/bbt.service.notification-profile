using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class AddLogTableToColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestDate",
                table: "LogDetails",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "LogId",
                table: "LogDetails",
                newName: "SourceId");

            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                table: "LogDetails",
                newName: "ResponseMessage");

            migrationBuilder.AddColumn<DateTime>(
                name: "ErrorDate",
                table: "Logs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestData",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseData",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerNo",
                table: "LogDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "LogDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotificationType",
                table: "LogDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "LogDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorDate",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "RequestData",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "ResponseData",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "CustomerNo",
                table: "LogDetails");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "LogDetails");

            migrationBuilder.DropColumn(
                name: "NotificationType",
                table: "LogDetails");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "LogDetails");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "LogDetails",
                newName: "LogId");

            migrationBuilder.RenameColumn(
                name: "ResponseMessage",
                table: "LogDetails",
                newName: "ErrorMessage");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "LogDetails",
                newName: "RequestDate");
        }
    }
}
