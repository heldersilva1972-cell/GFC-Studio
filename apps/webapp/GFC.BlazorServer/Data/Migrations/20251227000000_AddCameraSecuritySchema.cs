// [NEW]
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class AddCameraSecuritySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add columns to SystemSettings
            migrationBuilder.AddColumn<string>(
                name: "CloudflareTunnelToken",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicDomain",
                table: "SystemSettings",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WireGuardPort",
                table: "SystemSettings",
                type: "int",
                nullable: false,
                defaultValue: 51820);

            migrationBuilder.AddColumn<string>(
                name: "WireGuardSubnet",
                table: "SystemSettings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "10.8.0.0/24");

            // Create VpnProfiles table
            migrationBuilder.CreateTable(
                name: "VpnProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AssignedIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedBy = table.Column<int>(type: "int", nullable: true),
                    RevokedReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VpnProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VpnProfiles_AppUsers_RevokedBy",
                        column: x => x.RevokedBy,
                        principalTable: "AppUsers",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_VpnProfiles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create VpnSessions table
            migrationBuilder.CreateTable(
                name: "VpnSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VpnProfileId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ConnectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisconnectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BytesReceived = table.Column<long>(type: "bigint", nullable: false),
                    BytesSent = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VpnSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VpnSessions_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_VpnSessions_VpnProfiles_VpnProfileId",
                        column: x => x.VpnProfileId,
                        principalTable: "VpnProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create VideoAccessAudit table
            migrationBuilder.CreateTable(
                name: "VideoAccessAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccessType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CameraId = table.Column<int>(type: "int", nullable: true),
                    CameraName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ConnectionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SessionStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationSeconds = table.Column<int>(type: "int", nullable: true),
                    RecordingFile = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoAccessAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoAccessAudit_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoAccessAudit_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Create SecurityAlerts table
            migrationBuilder.CreateTable(
                name: "SecurityAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlertType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClientIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "New")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityAlerts_AppUsers_ReviewedBy",
                        column: x => x.ReviewedBy,
                        principalTable: "AppUsers",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_SecurityAlerts_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VpnProfiles_AssignedIP",
                table: "VpnProfiles",
                column: "AssignedIP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VpnProfiles_PublicKey",
                table: "VpnProfiles",
                column: "PublicKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VpnProfiles_RevokedBy",
                table: "VpnProfiles",
                column: "RevokedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VpnProfiles_UserId",
                table: "VpnProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VpnSessions_UserId",
                table: "VpnSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VpnSessions_VpnProfileId",
                table: "VpnSessions",
                column: "VpnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoAccessAudit_CameraId",
                table: "VideoAccessAudit",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoAccessAudit_UserId",
                table: "VideoAccessAudit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityAlerts_ReviewedBy",
                table: "SecurityAlerts",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityAlerts_UserId",
                table: "SecurityAlerts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VpnSessions");

            migrationBuilder.DropTable(
                name: "VideoAccessAudit");

            migrationBuilder.DropTable(
                name: "SecurityAlerts");

            migrationBuilder.DropTable(
                name: "VpnProfiles");

            migrationBuilder.DropColumn(
                name: "CloudflareTunnelToken",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "PublicDomain",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "WireGuardPort",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "WireGuardSubnet",
                table: "SystemSettings");
        }
    }
}
