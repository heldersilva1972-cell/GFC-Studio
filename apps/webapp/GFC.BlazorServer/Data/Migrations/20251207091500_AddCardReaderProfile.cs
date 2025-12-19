using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

// NOTE: After pulling, run EF migration update (e.g. dotnet ef database update).
/// <inheritdoc />
public partial class AddCardReaderProfile : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CardReaderProfiles",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DigitsOnly = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                MinLength = table.Column<int>(type: "int", nullable: true),
                MaxLength = table.Column<int>(type: "int", nullable: true),
                PrefixToTrim = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                SuffixToTrim = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                LastSampleRaw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastSampleParsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CardReaderProfiles", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CardReaderProfiles");
    }
}
