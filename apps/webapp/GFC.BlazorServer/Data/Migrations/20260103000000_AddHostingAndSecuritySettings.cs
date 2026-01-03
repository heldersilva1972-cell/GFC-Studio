// [NEW]
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class AddHostingAndSecuritySettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceTrustDurationDays",
                table: "SystemSettings",
                type: "int",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.AddColumn<bool>(
                name: "EnforceVpn",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HostingEnvironment",
                table: "SystemSettings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Dev");

            migrationBuilder.AddColumn<bool>(
                name: "MagicLinkEnabled",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceTrustDurationDays",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "EnforceVpn",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "HostingEnvironment",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "MagicLinkEnabled",
                table: "SystemSettings");
        }
    }
}
