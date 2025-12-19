using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

public partial class AddReimbursementSettingsTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Only create table if it doesn't exist (safe for databases that already have it from previous migrations)
        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementSettings', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementSettings](
        [Id] INT NOT NULL PRIMARY KEY,
        [ReceiptRequired] BIT NOT NULL DEFAULT(0),
        [NotificationRecipients] NVARCHAR(MAX) NULL
    );
END
");

        // Seed a single default row if table is empty
        migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM ReimbursementSettings)
BEGIN
    INSERT INTO ReimbursementSettings (Id, ReceiptRequired, NotificationRecipients)
    VALUES (1, 0, NULL);
END
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.ReimbursementSettings', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReimbursementSettings];
");
    }
}
