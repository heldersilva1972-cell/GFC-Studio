// [NEW]
using Microsoft.EntityFrameworkCore.Migrations;

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class AddFormFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "FormSubmissions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "Options",
                table: "FormFields");
        }
    }
}
