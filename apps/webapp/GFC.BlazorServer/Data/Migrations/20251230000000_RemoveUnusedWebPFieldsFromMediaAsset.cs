// [NEW]
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class RemoveUnusedWebPFieldsFromMediaAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath_AVIF",
                table: "MediaAssets");

            migrationBuilder.DropColumn(
                name: "FilePath_WebP",
                table: "MediaAssets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath_AVIF",
                table: "MediaAssets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath_WebP",
                table: "MediaAssets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
