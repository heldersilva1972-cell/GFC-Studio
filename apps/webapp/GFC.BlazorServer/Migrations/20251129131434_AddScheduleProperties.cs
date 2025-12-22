using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.RenameIndex(
            //     name: "IX_ControllerEvents_ControllerId_RawIndex",
            //     table: "ControllerEvents",
            //     newName: "IX_ControllerEvents_Controller_RawIndex");

            migrationBuilder.CreateTable(
                name: "DoorConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoorId = table.Column<int>(type: "int", nullable: false),
                    OpenTimeSeconds = table.Column<int>(type: "int", nullable: false),
                    LockDelaySeconds = table.Column<int>(type: "int", nullable: false),
                    AlarmEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoorConfigs_Doors_DoorId",
                        column: x => x.DoorId,
                        principalTable: "Doors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ControllerTimeProfileLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControllerId = table.Column<int>(type: "int", nullable: false),
                    TimeProfileId = table.Column<int>(type: "int", nullable: false),
                    ControllerProfileIndex = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControllerTimeProfileLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControllerTimeProfileLinks_Controllers_ControllerId",
                        column: x => x.ControllerId,
                        principalTable: "Controllers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ControllerTimeProfileLinks_TimeProfiles_TimeProfileId",
                        column: x => x.TimeProfileId,
                        principalTable: "TimeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeProfileIntervals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeProfileId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeProfileIntervals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeProfileIntervals_TimeProfiles_TimeProfileId",
                        column: x => x.TimeProfileId,
                        principalTable: "TimeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ControllerTimeProfileLinks_ControllerId_ControllerProfileIndex",
                table: "ControllerTimeProfileLinks",
                columns: new[] { "ControllerId", "ControllerProfileIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControllerTimeProfileLinks_ControllerId_TimeProfileId",
                table: "ControllerTimeProfileLinks",
                columns: new[] { "ControllerId", "TimeProfileId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControllerTimeProfileLinks_TimeProfileId",
                table: "ControllerTimeProfileLinks",
                column: "TimeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorConfigs_DoorId",
                table: "DoorConfigs",
                column: "DoorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_Date",
                table: "Holidays",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_TimeProfileIntervals_TimeProfileId_DayOfWeek_Order",
                table: "TimeProfileIntervals",
                columns: new[] { "TimeProfileId", "DayOfWeek", "Order" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ControllerTimeProfileLinks");

            migrationBuilder.DropTable(
                name: "DoorConfigs");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "TimeProfileIntervals");

            migrationBuilder.DropTable(
                name: "TimeProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_ControllerEvents_Controller_RawIndex",
                table: "ControllerEvents",
                newName: "IX_ControllerEvents_ControllerId_RawIndex");
        }
    }
}
