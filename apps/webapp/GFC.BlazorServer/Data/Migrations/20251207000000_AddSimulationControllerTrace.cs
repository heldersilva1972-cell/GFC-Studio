using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

/// <inheritdoc />
public partial class AddSimulationControllerTrace : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SimulationControllerTraces",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: true),
                Operation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                ControllerId = table.Column<int>(type: "int", nullable: true),
                DoorId = table.Column<int>(type: "int", nullable: true),
                CardNumber = table.Column<long>(type: "bigint", nullable: true),
                MemberId = table.Column<int>(type: "int", nullable: true),
                RequestSummary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                RequestPayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RequestPayloadRaw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ExpectedResponseSummary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                ExpectedResponsePayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ResultStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ResultDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                TriggerPage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IsSimulation = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SimulationControllerTraces", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SimulationControllerTraces_TimestampUtc",
            table: "SimulationControllerTraces",
            column: "TimestampUtc");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SimulationControllerTraces");
    }
}
