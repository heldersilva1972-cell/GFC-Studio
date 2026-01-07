-- Fix Controllers table schema to match the entity
USE [ClubMembership];
GO

-- Add missing columns to Controllers table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'DoorCount')
BEGIN
    ALTER TABLE [Controllers] ADD [DoorCount] int NOT NULL DEFAULT 4;
    PRINT 'Added DoorCount column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'NetworkType')
BEGIN
    ALTER TABLE [Controllers] ADD [NetworkType] nvarchar(50) NULL DEFAULT 'LAN';
    PRINT 'Added NetworkType column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'VpnProfileId')
BEGIN
    ALTER TABLE [Controllers] ADD [VpnProfileId] int NULL;
    PRINT 'Added VpnProfileId column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'BackupIpAddress')
BEGIN
    ALTER TABLE [Controllers] ADD [BackupIpAddress] nvarchar(50) NULL;
    PRINT 'Added BackupIpAddress column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'BackupPort')
BEGIN
    ALTER TABLE [Controllers] ADD [BackupPort] int NULL;
    PRINT 'Added BackupPort column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'BackupExpiresUtc')
BEGIN
    ALTER TABLE [Controllers] ADD [BackupExpiresUtc] datetime2 NULL;
    PRINT 'Added BackupExpiresUtc column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'LastMigrationUtc')
BEGIN
    ALTER TABLE [Controllers] ADD [LastMigrationUtc] datetime2 NULL;
    PRINT 'Added LastMigrationUtc column';
END

-- Fix SerialNumber type (int -> bigint to support uint)
-- Note: uint in C# is 0 to 4,294,967,295, which requires bigint in SQL Server
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'SerialNumber' AND system_type_id = 56) -- 56 = int
BEGIN
    -- Drop and recreate with correct type
    ALTER TABLE [Controllers] ALTER COLUMN [SerialNumber] bigint NOT NULL;
    PRINT 'Changed SerialNumber from int to bigint';
END

PRINT 'Controllers table schema updated successfully';
GO
