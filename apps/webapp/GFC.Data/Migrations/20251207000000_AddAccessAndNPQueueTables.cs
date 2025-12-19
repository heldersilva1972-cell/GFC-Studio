#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GFC.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessAndNPQueueTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    OldKeyCardId = table.Column<int>(type: "int", nullable: true),
                    NewKeyCardId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeyHistory_KeyCards_NewKeyCardId",
                        column: x => x.NewKeyCardId,
                        principalTable: "KeyCards",
                        principalColumn: "KeyCardId");
                    table.ForeignKey(
                        name: "FK_KeyHistory_KeyCards_OldKeyCardId",
                        column: x => x.OldKeyCardId,
                        principalTable: "KeyCards",
                        principalColumn: "KeyCardId");
                    table.ForeignKey(
                        name: "FK_KeyHistory_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberDoorAccess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    DoorId = table.Column<int>(type: "int", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TimeProfileId = table.Column<int>(type: "int", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSyncResult = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberDoorAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberDoorAccess_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberDoorAccess_TimeProfiles_TimeProfileId",
                        column: x => x.TimeProfileId,
                        principalTable: "TimeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "NPQueueEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeactivatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPQueueEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NPQueueEntries_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyHistory_MemberId",
                table: "KeyHistory",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyHistory_NewKeyCardId",
                table: "KeyHistory",
                column: "NewKeyCardId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyHistory_OldKeyCardId",
                table: "KeyHistory",
                column: "OldKeyCardId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberDoorAccess_MemberId",
                table: "MemberDoorAccess",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberDoorAccess_MemberId_DoorId_CardNumber",
                table: "MemberDoorAccess",
                columns: new[] { "MemberId", "DoorId", "CardNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_MemberDoorAccess_TimeProfileId",
                table: "MemberDoorAccess",
                column: "TimeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_NPQueueEntries_MemberId",
                table: "NPQueueEntries",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyHistory");

            migrationBuilder.DropTable(
                name: "MemberDoorAccess");

            migrationBuilder.DropTable(
                name: "NPQueueEntries");
        }
    }
}
