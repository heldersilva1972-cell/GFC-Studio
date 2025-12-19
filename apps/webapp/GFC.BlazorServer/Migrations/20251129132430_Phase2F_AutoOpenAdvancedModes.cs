using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Migrations
{
    /// <inheritdoc />
    public partial class Phase2F_AutoOpenAdvancedModes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ControllerBehaviorOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControllerId = table.Column<int>(type: "int", nullable: false),
                    ValidSwipeGapSeconds = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControllerBehaviorOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControllerBehaviorOptions_Controllers_ControllerId",
                        column: x => x.ControllerId,
                        principalTable: "Controllers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoorAutoOpenSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoorId = table.Column<int>(type: "int", nullable: false),
                    TimeProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ControllerProfileIndex = table.Column<int>(type: "int", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorAutoOpenSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoorAutoOpenSchedules_Doors_DoorId",
                        column: x => x.DoorId,
                        principalTable: "Doors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoorAutoOpenSchedules_TimeProfiles_TimeProfileId",
                        column: x => x.TimeProfileId,
                        principalTable: "TimeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DoorBehaviorOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoorId = table.Column<int>(type: "int", nullable: false),
                    FirstCardOpenEnabled = table.Column<bool>(type: "bit", nullable: false),
                    DoorAsSwitchEnabled = table.Column<bool>(type: "bit", nullable: false),
                    OpenTooLongWarnEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Invalid3CardsWarnEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorBehaviorOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoorBehaviorOptions_Doors_DoorId",
                        column: x => x.DoorId,
                        principalTable: "Doors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ControllerCommandInfos",
                keyColumn: "Id",
                keyValue: 12,
                column: "Phase",
                value: "Phase 2F");

            migrationBuilder.UpdateData(
                table: "ControllerCommandInfos",
                keyColumn: "Id",
                keyValue: 13,
                column: "Phase",
                value: "Phase 2F");

            migrationBuilder.CreateIndex(
                name: "IX_ControllerBehaviorOptions_ControllerId",
                table: "ControllerBehaviorOptions",
                column: "ControllerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoorAutoOpenSchedules_DoorId",
                table: "DoorAutoOpenSchedules",
                column: "DoorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoorAutoOpenSchedules_TimeProfileId",
                table: "DoorAutoOpenSchedules",
                column: "TimeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorBehaviorOptions_DoorId",
                table: "DoorBehaviorOptions",
                column: "DoorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ControllerBehaviorOptions");

            migrationBuilder.DropTable(
                name: "DoorAutoOpenSchedules");

            migrationBuilder.DropTable(
                name: "DoorBehaviorOptions");

            migrationBuilder.UpdateData(
                table: "ControllerCommandInfos",
                keyColumn: "Id",
                keyValue: 12,
                column: "Phase",
                value: "Phase 3D");

            migrationBuilder.UpdateData(
                table: "ControllerCommandInfos",
                keyColumn: "Id",
                keyValue: 13,
                column: "Phase",
                value: "Phase 3E");
        }
    }
}
