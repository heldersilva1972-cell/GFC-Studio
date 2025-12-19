using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

public partial class Fix_ControllerTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ControllerCommandInfos",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Phase = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ShortDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                LongDescription = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                RiskLevel = table.Column<int>(type: "int", nullable: false),
                IsReadOperation = table.Column<bool>(type: "bit", nullable: false),
                IsWriteOperation = table.Column<bool>(type: "bit", nullable: false),
                ProtocolInfo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Enabled = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ControllerCommandInfos", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Controllers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                SerialNumber = table.Column<long>(type: "bigint", nullable: false),
                IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Port = table.Column<int>(type: "int", nullable: false),
                IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Controllers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Doors",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ControllerId = table.Column<int>(type: "int", nullable: false),
                DoorIndex = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Doors", x => x.Id);
                table.ForeignKey(
                    name: "FK_Doors_Controllers_ControllerId",
                    column: x => x.ControllerId,
                    principalTable: "Controllers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ControllerLastIndexes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ControllerId = table.Column<int>(type: "int", nullable: false),
                LastRecordIndex = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ControllerLastIndexes", x => x.Id);
                table.ForeignKey(
                    name: "FK_ControllerLastIndexes_Controllers_ControllerId",
                    column: x => x.ControllerId,
                    principalTable: "Controllers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ControllerEvents",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ControllerId = table.Column<int>(type: "int", nullable: false),
                DoorId = table.Column<int>(type: "int", nullable: true),
                DoorIndex = table.Column<int>(type: "int", nullable: true),
                TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                CardNumber = table.Column<long>(type: "bigint", nullable: true),
                EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                ReasonCode = table.Column<int>(type: "int", nullable: false),
                IsByCard = table.Column<bool>(type: "bit", nullable: false),
                IsByButton = table.Column<bool>(type: "bit", nullable: false),
                RawIndex = table.Column<long>(type: "bigint", nullable: false),
                RawData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ControllerEvents", x => x.Id);
                table.ForeignKey(
                    name: "FK_ControllerEvents_Controllers_ControllerId",
                    column: x => x.ControllerId,
                    principalTable: "Controllers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ControllerEvents_Doors_DoorId",
                    column: x => x.DoorId,
                    principalTable: "Doors",
                    principalColumn: "Id");
            });

        migrationBuilder.InsertData(
            table: "ControllerCommandInfos",
            columns: new[]
            {
                "Id","Key","DisplayName","Category","Phase","ShortDescription","LongDescription","RiskLevel","IsReadOperation","IsWriteOperation","ProtocolInfo","Enabled"
            },
            values: new object[,]
            {
                { 1, "OpenDoor", "Open Door", "Door Control", "Phase 1", "Momentarily energize a door relay for a door or elevator output.", "Sends the Agent OpenDoor command which issues the Mengqi type 32/code 1 packet with optional duration override.", 1, false, true, "Agent POST /{sn}/door/{doorNo}/open", true },
                { 2, "SyncTime", "Sync Controller Time", "Timekeeping", "Phase 1", "Align the controller RTC with the server clock.", "Uses the AdjustTime command (type 32/code 48) via the Agent to ensure logs timestamps stay accurate.", 0, false, true, "Agent POST /{sn}/sync-time", true },
                { 3, "AddOrUpdateCard", "Add or Update Card", "Privileges", "Phase 3A", "Apply a single card privilege row to the controller.", "Wraps the Mengqi privilege packet (type 36/code 62) through the Agent bulk/individual privilege APIs.", 1, false, true, "Agent POST /{sn}/cards", true },
                { 4, "DeleteCard", "Delete Card", "Privileges", "Phase 3A", "Remove a single card slot from the controller.", "Marks the privilege row as deleted via the Agent DELETE /cards/{cardNo} endpoint.", 1, false, true, "Agent DELETE /{sn}/cards/{cardNo}", true },
                { 5, "ClearAllCards", "Clear All Cards", "Privileges", "Phase 3A", "Erase the entire privilege table.", "Triggers the dangerous type 36/code 64 command through the Agent ClearAllCards API.", 3, false, true, "Agent POST /{sn}/cards/clear-all", true },
                { 6, "BulkUploadCards", "Bulk Upload Cards", "Privileges", "Phase 3A", "Stream a batch of privilege rows to the controller.", "Uses the Agent bulk upload endpoint which internally sequences the WGPacket privilege writes.", 2, false, true, "Agent POST /{sn}/cards/bulk-upload", true },
                { 7, "ReadEvents", "Read Events", "Monitoring", "Phase 2", "Fetch swipe/event records since the last index.", "Calls Agent GET /events which wraps GetLastRecordIndex + GetSingleSwipeRecord + confirm index.", 0, true, false, "Agent GET /{sn}/events?lastIndex=x", true },
                { 8, "ReadRunStatus", "Read Run/Status Block", "Monitoring", "Phase 2", "Poll live door, relay, and alarm states.", "Uses Agent GET /run-status which relays the Mengqi run info command (type 32/code 32).", 0, true, false, "Agent GET /{sn}/run-status", true },
                { 9, "SyncDoorConfig", "Sync Door Configuration", "Configuration", "Phase 3B", "Write door timing/mode settings.", "Uses Agent POST /door-config/sync which maps to extended config flash writes.", 2, false, true, "Agent POST /{sn}/door-config/sync", true },
                { 10, "SyncTimeProfiles", "Sync Time Profiles", "Schedules", "Phase 3C", "Write controller time zones / schedules.", "Wraps the Agent POST /time-schedules/sync endpoint that fills the holiday/time zone blocks.", 2, false, true, "Agent POST /{sn}/time-schedules/sync", true },
                { 11, "SyncHolidays", "Sync Holidays", "Schedules", "Phase 3C", "Update the holiday block used by privilege filtering.", "Part of the same FLASH region as time profiles; triggered through the schedule sync API.", 2, false, true, "Included in POST /{sn}/time-schedules/sync", true },
                { 12, "SyncAutoOpen", "Sync Auto Open Tasks", "Schedules", "Phase 3D", "Write automatic door unlock/lock tasks.", "Uses the Agent POST /auto-open/sync endpoint to replace the task table.", 2, false, true, "Agent POST /{sn}/auto-open/sync", true },
                { 13, "SyncAdvancedDoorModes", "Sync Advanced Door Modes", "Configuration", "Phase 3E", "Write extended per-door options (double lock, first card, etc.).", "Maps to Agent POST /advanced-door-modes/sync which writes the extended FRAM structures.", 2, false, true, "Agent POST /{sn}/advanced-door-modes/sync", true },
                { 14, "RebootController", "Reboot Controller", "Maintenance", "Phase 5", "Soft reboot the panel.", "Agent POST /reboot triggers the Mengqi special command (type 32/code 254).", 3, false, true, "Agent POST /{sn}/reboot", true },
                { 15, "NetworkConfig", "Read/Write Network Config", "Configuration", "Phase 5B", "Read or set controller IP/allowed PC blocks.", "Covers Agent GET/POST /network-config which wraps the AutoIPSet and PCAllowed operations.", 3, true, true, "Agent GET/POST /{sn}/network-config", true }
            });

        migrationBuilder.CreateIndex(
            name: "IX_ControllerCommandInfos_Key",
            table: "ControllerCommandInfos",
            column: "Key",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ControllerEvents_ControllerId_RawIndex",
            table: "ControllerEvents",
            columns: new[] { "ControllerId", "RawIndex" });

        migrationBuilder.CreateIndex(
            name: "IX_ControllerEvents_DoorId",
            table: "ControllerEvents",
            column: "DoorId");

        migrationBuilder.CreateIndex(
            name: "IX_ControllerLastIndexes_ControllerId",
            table: "ControllerLastIndexes",
            column: "ControllerId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Controllers_SerialNumber",
            table: "Controllers",
            column: "SerialNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Doors_ControllerId_DoorIndex",
            table: "Doors",
            columns: new[] { "ControllerId", "DoorIndex" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ControllerCommandInfos");

        migrationBuilder.DropTable(
            name: "ControllerEvents");

        migrationBuilder.DropTable(
            name: "ControllerLastIndexes");

        migrationBuilder.DropTable(
            name: "Doors");

        migrationBuilder.DropTable(
            name: "Controllers");
    }
}

