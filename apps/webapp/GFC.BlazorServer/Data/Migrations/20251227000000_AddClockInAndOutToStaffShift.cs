// [NEW]
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class AddClockInAndOutToStaffShift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClockInTime",
                table: "StaffShifts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClockOutTime",
                table: "StaffShifts",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClockInTime",
                table: "StaffShifts");

            migrationBuilder.DropColumn(
                name: "ClockOutTime",
                table: "StaffShifts");
        }
    }
}
