using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

/// <inheritdoc />
public partial class AddDoorConfigTimestamps : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedUtc",
            table: "DoorConfigs",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedUtc",
            table: "DoorConfigs",
            type: "datetime2",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CreatedUtc",
            table: "DoorConfigs");

        migrationBuilder.DropColumn(
            name: "UpdatedUtc",
            table: "DoorConfigs");
    }
}

