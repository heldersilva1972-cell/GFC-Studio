// [NEW]
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class AddAssetManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetFolders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentFolderId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetFolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetFolders_AssetFolders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "AssetFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MediaAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath_xl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath_lg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath_md = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath_sm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath_WebP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath_AVIF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetFolderId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaAssets_AssetFolders_AssetFolderId",
                        column: x => x.AssetFolderId,
                        principalTable: "AssetFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaAssets_AssetFolderId",
                table: "MediaAssets",
                column: "AssetFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetFolders_ParentFolderId",
                table: "AssetFolders",
                column: "ParentFolderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaAssets");

            migrationBuilder.DropTable(
                name: "AssetFolders");
        }
    }
}
