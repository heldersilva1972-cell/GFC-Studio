// [NEW]
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudioSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing foreign keys and indexes if they exist
            migrationBuilder.DropForeignKey(
                name: "FK_StudioSections_StudioPages_StudioPageId",
                table: "StudioSections");

            migrationBuilder.DropIndex(
                name: "IX_StudioSections_StudioPageId",
                table: "StudioSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudioPages",
                table: "StudioPages");

            migrationBuilder.DropColumn(name: "StudioPageId", table: "StudioSections");
            migrationBuilder.DropColumn(name: "Content", table: "StudioPages");
            migrationBuilder.DropColumn(name: "IsPublished", table: "StudioPages");
            migrationBuilder.DropColumn(name: "LastPublishedAt", table: "StudioPages");

            // Recreate StudioPages table with correct columns
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StudioPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudioPages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "StudioPages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StudioPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudioPages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "StudioPages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "StudioPages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OgImage",
                table: "StudioPages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "StudioPages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublishedBy",
                table: "StudioPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StudioPages",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Draft");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudioPages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "StudioPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudioPages",
                table: "StudioPages",
                column: "Id");

            // Recreate StudioSections table with correct columns
            migrationBuilder.AddColumn<int>(
                name: "PageId",
                table: "StudioSections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ComponentType",
                table: "StudioSections",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentJson",
                table: "StudioSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "StudioSections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StylesJson",
                table: "StudioSections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnimationJson",
                table: "StudioSections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsiveJson",
                table: "StudioSections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "StudioSections",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibleOnDesktop",
                table: "StudioSections",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibleOnTablet",
                table: "StudioSections",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibleOnMobile",
                table: "StudioSections",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudioSections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StudioSections",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudioSections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "StudioSections",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudioSections_PageId",
                table: "StudioSections",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudioSections_StudioPages_PageId",
                table: "StudioSections",
                column: "PageId",
                principalTable: "StudioPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Update StudioDrafts table
            migrationBuilder.AddColumn<string>(
                name: "ChangeDescription",
                table: "StudioDrafts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContentJson",
                table: "StudioDrafts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "StudioDrafts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Create StudioSettings table
            migrationBuilder.CreateTable(
                name: "StudioSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SettingType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudioSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudioSettings_SettingKey",
                table: "StudioSettings",
                column: "SettingKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudioSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_StudioSections_StudioPages_PageId",
                table: "StudioSections");

            migrationBuilder.DropIndex(
                name: "IX_StudioSections_PageId",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "ComponentType",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "ContentJson",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "StylesJson",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "AnimationJson",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "ResponsiveJson",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "VisibleOnDesktop",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "VisibleOnTablet",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "VisibleOnMobile",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StudioSections");

            migrationBuilder.DropColumn(
                name: "ChangeDescription",
                table: "StudioDrafts");

            migrationBuilder.AlterColumn<string>(
                name: "ContentJson",
                table: "StudioDrafts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "StudioDrafts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudioPages",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "OgImage",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "PublishedBy",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StudioPages");

            migrationBuilder.AddColumn<int>(
                name: "StudioPageId",
                table: "StudioSections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "StudioPages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "StudioPages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPublishedAt",
                table: "StudioPages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudioPages",
                table: "StudioPages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudioSections_StudioPageId",
                table: "StudioSections",
                column: "StudioPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudioSections_StudioPages_StudioPageId",
                table: "StudioSections",
                column: "StudioPageId",
                principalTable: "StudioPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
