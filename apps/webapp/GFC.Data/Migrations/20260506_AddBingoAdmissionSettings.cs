using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.Data.Migrations
{
    public partial class AddBingoAdmissionSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BingoBaseAdmissionPrice",
                table: "SystemSettings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 15.00m);

            migrationBuilder.AddColumn<decimal>(
                name: "BingoAdditionalSquarePrice",
                table: "SystemSettings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 1.00m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BingoBaseAdmissionPrice",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "BingoAdditionalSquarePrice",
                table: "SystemSettings");
        }
    }
}
