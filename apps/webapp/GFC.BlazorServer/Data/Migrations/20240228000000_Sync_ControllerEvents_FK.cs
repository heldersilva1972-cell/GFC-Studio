using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

public partial class Sync_ControllerEvents_FK : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("-- ControllerEvents FK configuration synchronized with existing database.");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}

