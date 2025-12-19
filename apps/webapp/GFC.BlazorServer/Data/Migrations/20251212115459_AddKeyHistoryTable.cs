using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

/// <inheritdoc />
public partial class AddKeyHistoryTable : Migration
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
                CardNumber = table.Column<long>(type: "bigint", nullable: false),
                Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KeyHistory", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_KeyHistory_CardNumber",
            table: "KeyHistory",
            column: "CardNumber");

        migrationBuilder.CreateIndex(
            name: "IX_KeyHistory_Date",
            table: "KeyHistory",
            column: "Date");

        migrationBuilder.CreateIndex(
            name: "IX_KeyHistory_MemberId",
            table: "KeyHistory",
            column: "MemberId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "KeyHistory");
    }
}
