using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class UpdateSourceLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourceLogs_Sources_ParentId",
                table: "SourceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_SourceParameter_SourceLogs_SourceLogId",
                table: "SourceParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_SourceLogs_SourceLogId",
                table: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_Sources_SourceLogId",
                table: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_SourceParameter_SourceLogId",
                table: "SourceParameter");

            migrationBuilder.DropIndex(
                name: "IX_SourceLogs_ParentId",
                table: "SourceLogs");

            migrationBuilder.DropColumn(
                name: "SourceLogId",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "SourceLogId",
                table: "SourceParameter");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceLogId",
                table: "Sources",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceLogId",
                table: "SourceParameter",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sources_SourceLogId",
                table: "Sources",
                column: "SourceLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceParameter_SourceLogId",
                table: "SourceParameter",
                column: "SourceLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceLogs_ParentId",
                table: "SourceLogs",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SourceLogs_Sources_ParentId",
                table: "SourceLogs",
                column: "ParentId",
                principalTable: "Sources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SourceParameter_SourceLogs_SourceLogId",
                table: "SourceParameter",
                column: "SourceLogId",
                principalTable: "SourceLogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_SourceLogs_SourceLogId",
                table: "Sources",
                column: "SourceLogId",
                principalTable: "SourceLogs",
                principalColumn: "Id");
        }
    }
}
