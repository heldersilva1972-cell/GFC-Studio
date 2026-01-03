-- Protocol: Manual Fix Script for Magic Link & System Settings
-- Created: 2026-01-02
-- Usage: Run this script in SSMS to apply pending schema changes manually.

BEGIN TRANSACTION;

-- 1. Add MagicLinkTokens Table if not exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MagicLinkTokens]') AND type in (N'U'))
BEGIN
    CREATE TABLE [MagicLinkTokens] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [Token] nvarchar(256) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [ExpiresAtUtc] datetime2 NOT NULL,
        [IsUsed] bit NOT NULL,
        [IpAddress] nvarchar(50) NOT NULL DEFAULT N'',
        CONSTRAINT [PK_MagicLinkTokens] PRIMARY KEY ([Id])
    );
    
    -- Add index for fast token lookup
    CREATE INDEX [IX_MagicLinkTokens_Token] ON [MagicLinkTokens] ([Token]);
    
    PRINT 'Created MagicLinkTokens table.';
END
ELSE
BEGIN
    PRINT 'MagicLinkTokens table already exists.';
END

-- 2. Add LanSubnet to SystemSettings if missing
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'SystemSettings')
BEGIN
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'LanSubnet' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [LanSubnet] nvarchar(max) NULL;
        PRINT 'Added LanSubnet column to SystemSettings.';
    END
    ELSE
    BEGIN
        PRINT 'LanSubnet column already exists.';
    END
END

-- 3. Add WireGuardSubnet to SystemSettings if missing (part of recent changes)
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'SystemSettings')
BEGIN
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'WireGuardSubnet' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [WireGuardSubnet] nvarchar(max) NULL;
        PRINT 'Added WireGuardSubnet column to SystemSettings.';
    END
END

-- 4. Add EnforceVpn to SystemSettings if missing
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'SystemSettings')
BEGIN
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnforceVpn' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [EnforceVpn] bit NOT NULL DEFAULT 0;
        PRINT 'Added EnforceVpn column to SystemSettings.';
    END
END

COMMIT;
PRINT 'Database schema update completed successfully.';
