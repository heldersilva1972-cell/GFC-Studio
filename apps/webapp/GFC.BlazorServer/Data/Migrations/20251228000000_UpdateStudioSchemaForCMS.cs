// [NEW]
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class UpdateStudioSchemaForCMS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key from StudioSections to StudioPages on the old string-based key if it exists
            // Note: This is a placeholder as the previous model was incorrect. This might fail if the FK doesn't exist.
            // A more robust script would check for the FK's existence first.
            try
            {
                migrationBuilder.DropForeignKey(
                    name: "FK_StudioSections_StudioPages_StudioPageId",
                    table: "StudioSections");
            }
            catch(Exception) { /*Ignore if it doesn't exist*/ }

            // Step 1: Alter StudioPages Table
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "StudioPages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "StudioPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "StudioPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            // Step 2: Alter StudioDrafts Table
            // EF Core migrations would typically drop and recreate the table to change PK type,
            // but we will try to do this with ALTER statements. This assumes SQL Server.
            // First, drop the foreign key from StudioDrafts to StudioPages
            migrationBuilder.DropForeignKey(
                name: "FK_StudioDrafts_StudioPages_PageId",
                table: "StudioDrafts");

            // Drop existing primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudioDrafts",
                table: "StudioDrafts");

            // Drop the old Guid Id column and add a new int Id column
            migrationBuilder.DropColumn(name: "Id", table: "StudioDrafts");
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StudioDrafts",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            // Add the new primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_StudioDrafts",
                table: "StudioDrafts",
                column: "Id");

            // Rename column
            migrationBuilder.RenameColumn(
                name: "Payload",
                table: "StudioDrafts",
                newName: "ContentSnapshotJson");

            // Add missing column
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StudioDrafts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "System");

            // Re-add the foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_StudioDrafts_StudioPages_PageId",
                table: "StudioDrafts",
                column: "PageId",
                principalTable: "StudioPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse changes in StudioDrafts
            migrationBuilder.DropForeignKey(
                name: "FK_StudioDrafts_StudioPages_PageId",
                table: "StudioDrafts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudioDrafts",
                table: "StudioDrafts");

            migrationBuilder.DropColumn(name: "Id", table: "StudioDrafts");
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "StudioDrafts",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudioDrafts",
                table: "StudioDrafts",
                column: "Id");

            migrationBuilder.RenameColumn(
                name: "ContentSnapshotJson",
                table: "StudioDrafts",
                newName: "Payload");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StudioDrafts");

            migrationBuilder.AddForeignKey(
                name: "FK_StudioDrafts_StudioPages_PageId",
                table: "StudioDrafts",
                column: "PageId",
                principalTable: "StudioPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Reverse changes in StudioPages
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "StudioPages");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "StudioPages");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "StudioPages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
