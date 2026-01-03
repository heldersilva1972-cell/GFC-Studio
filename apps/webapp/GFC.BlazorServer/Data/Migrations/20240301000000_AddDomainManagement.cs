// [NEW]
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class AddDomainManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicDomain",
                table: "SystemSettings",
                newName: "PrimaryDomain");

            migrationBuilder.AddColumn<string>(
                name: "AllowedDomains",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DomainSwitchPending",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DomainSwitchExpiryUtc",
                table: "SystemSettings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastConfirmedDomain",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrimaryDomain",
                table: "SystemSettings",
                newName: "PublicDomain");

            migrationBuilder.DropColumn(
                name: "AllowedDomains",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "DomainSwitchPending",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "DomainSwitchExpiryUtc",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "LastConfirmedDomain",
                table: "SystemSettings");
        }
    }
}
