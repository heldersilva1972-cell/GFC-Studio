using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

/// <inheritdoc />
public partial class AddIsSimulatedToControllerEvent : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsSimulated",
            table: "ControllerEvents",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsSimulated",
            table: "ControllerEvents");
    }
}
