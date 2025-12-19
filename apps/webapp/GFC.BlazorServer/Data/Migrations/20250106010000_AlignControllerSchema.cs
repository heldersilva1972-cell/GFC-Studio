using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

/// <inheritdoc />
public partial class AlignControllerSchema : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Ensure ControllerNetworkConfigs table exists with expected schema
        migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControllerNetworkConfigs]') AND type = N'U')
BEGIN
    CREATE TABLE [dbo].[ControllerNetworkConfigs](
        [Id] INT IDENTITY(1,1) NOT NULL,
        [ControllerId] INT NOT NULL,
        [IpAddress] NVARCHAR(45) NULL,
        [SubnetMask] NVARCHAR(45) NULL,
        [Gateway] NVARCHAR(45) NULL,
        [Port] INT NULL,
        [DhcpEnabled] BIT NOT NULL,
        [AllowedPcIp] NVARCHAR(45) NULL,
        [CommPasswordMasked] NVARCHAR(50) NULL,
        [LastReadUtc] DATETIME2 NULL,
        [LastSyncUtc] DATETIME2 NULL,
        [CreatedUtc] DATETIME2 NOT NULL,
        [UpdatedUtc] DATETIME2 NULL,
        CONSTRAINT [PK_ControllerNetworkConfigs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ControllerNetworkConfigs_Controllers_ControllerId] FOREIGN KEY ([ControllerId]) REFERENCES [dbo].[Controllers]([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ControllerNetworkConfigs_ControllerId' AND object_id = OBJECT_ID(N'[dbo].[ControllerNetworkConfigs]'))
BEGIN
    CREATE UNIQUE INDEX [IX_ControllerNetworkConfigs_ControllerId]
    ON [dbo].[ControllerNetworkConfigs]([ControllerId]);
END;
");

        // Ensure DoorConfigs has timestamp columns if missing
        migrationBuilder.Sql(@"
IF COL_LENGTH('DoorConfigs', 'CreatedUtc') IS NULL
BEGIN
    ALTER TABLE [dbo].[DoorConfigs] ADD [CreatedUtc] DATETIME2 NULL;
END;

IF COL_LENGTH('DoorConfigs', 'UpdatedUtc') IS NULL
BEGIN
    ALTER TABLE [dbo].[DoorConfigs] ADD [UpdatedUtc] DATETIME2 NULL;
END;
");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Drop timestamp columns only if this migration added them
        migrationBuilder.Sql(@"
IF COL_LENGTH('DoorConfigs', 'CreatedUtc') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[DoorConfigs] DROP COLUMN [CreatedUtc];
END;

IF COL_LENGTH('DoorConfigs', 'UpdatedUtc') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[DoorConfigs] DROP COLUMN [UpdatedUtc];
END;
");

        // Drop ControllerNetworkConfigs table if it exists
        migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControllerNetworkConfigs]') AND type = N'U')
BEGIN
    DROP TABLE [dbo].[ControllerNetworkConfigs];
END;
");
    }
}
