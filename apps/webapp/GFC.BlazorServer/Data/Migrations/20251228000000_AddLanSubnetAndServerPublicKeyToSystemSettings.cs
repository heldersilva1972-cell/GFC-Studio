// [NEW]
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class AddLanSubnetAndServerPublicKeyToSystemSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LanSubnet",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "192.168.1.0/24");

            migrationBuilder.AddColumn<string>(
                name: "WireGuardServerPublicKey",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LanSubnet",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "WireGuardServerPublicKey",
                table: "SystemSettings");
        }
    }
}
