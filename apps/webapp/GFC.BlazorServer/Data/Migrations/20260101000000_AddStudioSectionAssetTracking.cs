// [NEW]
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class AddStudioSectionAssetTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudioSectionAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudioSectionId = table.Column<int>(type: "int", nullable: false),
                    MediaAssetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudioSectionAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudioSectionAssets_Sections_StudioSectionId",
                        column: x => x.StudioSectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudioSectionAssets_MediaAssets_MediaAssetId",
                        column: x => x.MediaAssetId,
                        principalTable: "MediaAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudioSectionAssets_StudioSectionId",
                table: "StudioSectionAssets",
                column: "StudioSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudioSectionAssets_MediaAssetId",
                table: "StudioSectionAssets",
                column: "MediaAssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudioSectionAssets");
        }
    }
}
