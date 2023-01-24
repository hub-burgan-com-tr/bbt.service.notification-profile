using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Profile.Migrations
{
    public partial class AddSourceLogsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "SourceLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title_TR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    DisplayType = table.Column<int>(type: "int", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KafkaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientIdJsonPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Secret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PushServiceReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmsServiceReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailServiceReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KafkaCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetentationTime = table.Column<int>(type: "int", nullable: false),
                    ProductCodeId = table.Column<int>(type: "int", nullable: true),
                    SaveInbox = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Environment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceLogs_Sources_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sources",
                        principalColumn: "Id");
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SourceLogsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourceParameter_SourceLogs_SourceLogId",
                table: "SourceParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_SourceLogs_SourceLogId",
                table: "Sources");

            migrationBuilder.DropTable(
                name: "SourceLogs")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SourceLogsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropIndex(
                name: "IX_Sources_SourceLogId",
                table: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_SourceParameter_SourceLogId",
                table: "SourceParameter");

            migrationBuilder.DropColumn(
                name: "SourceLogId",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "SourceLogId",
                table: "SourceParameter");
        }
    }
}
