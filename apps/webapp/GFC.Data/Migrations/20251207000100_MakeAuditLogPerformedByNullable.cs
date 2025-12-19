#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace GFC.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeAuditLogPerformedByNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_PerformedBy",
                table: "AuditLogs");

            migrationBuilder.AlterColumn<int>(
                name: "PerformedByUserId",
                table: "AuditLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_PerformedBy",
                table: "AuditLogs",
                column: "PerformedByUserId",
                principalTable: "AppUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_PerformedBy",
                table: "AuditLogs");

            migrationBuilder.AlterColumn<int>(
                name: "PerformedByUserId",
                table: "AuditLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_PerformedBy",
                table: "AuditLogs",
                column: "PerformedByUserId",
                principalTable: "AppUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
