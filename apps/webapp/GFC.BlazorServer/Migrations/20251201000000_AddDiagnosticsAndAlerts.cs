// [NEW]
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Migrations
{
    public partial class AddDiagnosticsAndAlerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertThresholds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetricType = table.Column<int>(type: "int", nullable: false),
                    AlertLevel = table.Column<int>(type: "int", nullable: false),
                    ThresholdValue = table.Column<double>(type: "float", nullable: false),
                    CooldownMinutes = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertThresholds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CpuUsage = table.Column<double>(type: "float", nullable: false),
                    MemoryUsageGb = table.Column<double>(type: "float", nullable: false),
                    MemoryUsagePercentage = table.Column<double>(type: "float", nullable: false),
                    ActiveThreads = table.Column<int>(type: "int", nullable: false),
                    ActiveConnections = table.Column<int>(type: "int", nullable: false),
                    RequestsPerMinute = table.Column<int>(type: "int", nullable: false),
                    Gen0Collections = table.Column<int>(type: "int", nullable: false),
                    Gen1Collections = table.Column<int>(type: "int", nullable: false),
                    Gen2Collections = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiagnosticAlerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlertThresholdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TriggerValue = table.Column<double>(type: "float", nullable: false),
                    IsAcknowledged = table.Column<bool>(type: "bit", nullable: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcknowledgedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosticAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiagnosticAlerts_AlertThresholds_AlertThresholdId",
                        column: x => x.AlertThresholdId,
                        principalTable: "AlertThresholds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertThresholds_MetricType_AlertLevel",
                table: "AlertThresholds",
                columns: new[] { "MetricType", "AlertLevel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticAlerts_AlertThresholdId",
                table: "DiagnosticAlerts",
                column: "AlertThresholdId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticAlerts_Timestamp",
                table: "DiagnosticAlerts",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceSnapshots_Timestamp",
                table: "PerformanceSnapshots",
                column: "Timestamp");

            migrationBuilder.InsertData(
                table: "AlertThresholds",
                columns: new[] { "Id", "MetricType", "AlertLevel", "ThresholdValue", "CooldownMinutes", "IsEnabled" },
                values: new object[,]
                {
                    { Guid.NewGuid(), 0, 0, 80.0, 15, true },
                    { Guid.NewGuid(), 0, 1, 95.0, 5, true },
                    { Guid.NewGuid(), 1, 0, 85.0, 15, true },
                    { Guid.NewGuid(), 1, 1, 95.0, 5, true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiagnosticAlerts");

            migrationBuilder.DropTable(
                name: "PerformanceSnapshots");

            migrationBuilder.DropTable(
                name: "AlertThresholds");
        }
    }
}
