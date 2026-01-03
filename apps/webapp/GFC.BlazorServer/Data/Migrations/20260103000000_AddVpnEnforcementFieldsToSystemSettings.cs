// [NEW]
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVpnEnforcementFieldsToSystemSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessMode",
                table: "SystemSettings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Open");

            migrationBuilder.AddColumn<bool>(
                name: "EnforceVpn",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessMode",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "EnforceVpn",
                table: "SystemSettings");
        }
    }
}
