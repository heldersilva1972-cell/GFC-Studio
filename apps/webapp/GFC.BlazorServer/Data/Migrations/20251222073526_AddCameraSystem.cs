using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCameraSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimulationControllerTraces");

            migrationBuilder.DropColumn(
                name: "UseRealControllers",
                table: "SystemSettings");

            migrationBuilder.AddColumn<int>(
                name: "ScannerControllerId",
                table: "SystemSettings",
                type: "int",
                nullable: true);

            // AppUsers table already exists, skipping creation
            /*
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordChangeRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.UserId);
                });
            */

            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RtspUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserNotificationPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReimbursementNotifyEmail = table.Column<bool>(type: "bit", nullable: false),
                    ReimbursementNotifySMS = table.Column<bool>(type: "bit", nullable: false),
                    MemberSignupNotifyEmail = table.Column<bool>(type: "bit", nullable: false),
                    MemberSignupNotifySMS = table.Column<bool>(type: "bit", nullable: false),
                    DuesPaymentNotifyEmail = table.Column<bool>(type: "bit", nullable: false),
                    DuesPaymentNotifySMS = table.Column<bool>(type: "bit", nullable: false),
                    SystemAlertNotifyEmail = table.Column<bool>(type: "bit", nullable: false),
                    SystemAlertNotifySMS = table.Column<bool>(type: "bit", nullable: false),
                    LotterySalesNotifyEmail = table.Column<bool>(type: "bit", nullable: false),
                    LotterySalesNotifySMS = table.Column<bool>(type: "bit", nullable: false),
                    ControllerEventNotifyEmail = table.Column<bool>(type: "bit", nullable: false),
                    ControllerEventNotifySMS = table.Column<bool>(type: "bit", nullable: false),
                    NotificationReminderDismissed = table.Column<bool>(type: "bit", nullable: false),
                    NotificationReminderDismissedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotificationPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CameraAuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CameraId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CameraAuditLogs_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CameraAuditLogs_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CameraEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CameraId = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CameraEvents_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CameraPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CameraId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccessLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CameraPermissions_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CameraPermissions_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recordings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CameraId = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recordings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recordings_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "ScannerControllerId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_CameraAuditLogs_CameraId",
                table: "CameraAuditLogs",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraAuditLogs_UserId",
                table: "CameraAuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraEvents_CameraId",
                table: "CameraEvents",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraPermissions_CameraId",
                table: "CameraPermissions",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraPermissions_UserId",
                table: "CameraPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recordings_CameraId",
                table: "Recordings",
                column: "CameraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameraAuditLogs");

            migrationBuilder.DropTable(
                name: "CameraEvents");

            migrationBuilder.DropTable(
                name: "CameraPermissions");

            migrationBuilder.DropTable(
                name: "Recordings");

            migrationBuilder.DropTable(
                name: "UserNotificationPreferences");

            // AppUsers table was not created by this migration, skipping drop
            /*
            migrationBuilder.DropTable(
                name: "AppUsers");
            */

            migrationBuilder.DropTable(
                name: "Cameras");

            migrationBuilder.DropColumn(
                name: "ScannerControllerId",
                table: "SystemSettings");

            migrationBuilder.AddColumn<bool>(
                name: "UseRealControllers",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SimulationControllerTraces",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<long>(type: "bigint", nullable: true),
                    ControllerId = table.Column<int>(type: "int", nullable: true),
                    DoorId = table.Column<int>(type: "int", nullable: true),
                    ExpectedResponsePayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedResponseSummary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsSimulation = table.Column<bool>(type: "bit", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    Operation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestPayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestPayloadRaw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestSummary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResultDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TriggerPage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationControllerTraces", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UseRealControllers",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_SimulationControllerTraces_TimestampUtc",
                table: "SimulationControllerTraces",
                column: "TimestampUtc");
        }
    }
}
