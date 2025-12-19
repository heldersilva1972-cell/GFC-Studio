using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

/// <inheritdoc />
public partial class AddControllerNetworkConfig : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ControllerNetworkConfigs",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ControllerId = table.Column<int>(type: "int", nullable: false),
                IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                SubnetMask = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                Gateway = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                Port = table.Column<int>(type: "int", nullable: true),
                DhcpEnabled = table.Column<bool>(type: "bit", nullable: false),
                AllowedPcIp = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                CommPasswordMasked = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                LastReadUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastSyncUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ControllerNetworkConfigs", x => x.Id);
                table.ForeignKey(
                    name: "FK_ControllerNetworkConfigs_Controllers_ControllerId",
                    column: x => x.ControllerId,
                    principalTable: "Controllers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ControllerNetworkConfigs_ControllerId",
            table: "ControllerNetworkConfigs",
            column: "ControllerId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ControllerNetworkConfigs");
    }
}
