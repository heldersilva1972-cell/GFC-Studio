USE [ClubMembership];
GO

BEGIN TRANSACTION;

-- Add missing columns to SystemSettings if they don't exist

IF EXISTS(SELECT * FROM sys.tables WHERE name = 'SystemSettings')
BEGIN
    PRINT 'Checking SystemSettings table for missing columns...';

    -- Backup & Data Protection Tracking (Lines 106-109 in SystemSettings.cs)
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'BackupMethod' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [BackupMethod] nvarchar(max) NOT NULL DEFAULT N'External USB';
        PRINT 'Added BackupMethod';
    END
    
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'LastSuccessfulBackupUtc' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [LastSuccessfulBackupUtc] datetime2 NULL;
        PRINT 'Added LastSuccessfulBackupUtc';
    END

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'LastRestoreTestUtc' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [LastRestoreTestUtc] datetime2 NULL;
        PRINT 'Added LastRestoreTestUtc';
    END

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'BackupFrequencyHours' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [BackupFrequencyHours] int NOT NULL DEFAULT 24;
        PRINT 'Added BackupFrequencyHours';
    END

    -- Database Maintenance Settings (Lines 113-120 in SystemSettings.cs)
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'BackupStoragePath' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [BackupStoragePath] nvarchar(500) NOT NULL DEFAULT N'C:\GFC_Backups\Sql\';
        PRINT 'Added BackupStoragePath';
    END
    
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'BackupRetentionCount' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [BackupRetentionCount] int NOT NULL DEFAULT 10;
        PRINT 'Added BackupRetentionCount';
    END
    
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'AllowServerRestoreOperations' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [AllowServerRestoreOperations] bit NOT NULL DEFAULT 0;
        PRINT 'Added AllowServerRestoreOperations';
    END
    
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MaintenanceModeEnabled' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [MaintenanceModeEnabled] bit NOT NULL DEFAULT 0;
        PRINT 'Added MaintenanceModeEnabled';
    END

    -- Safe Mode (Line 103)
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'SafeModeEnabled' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [SafeModeEnabled] bit NOT NULL DEFAULT 0;
        PRINT 'Added SafeModeEnabled';
    END
    
    -- Safety check for other potentially missing fields from recent updates
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MagicLinkEnabled' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [MagicLinkEnabled] bit NOT NULL DEFAULT 1;
        PRINT 'Added MagicLinkEnabled';
    END

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnforceVpn' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [EnforceVpn] bit NOT NULL DEFAULT 0;
        PRINT 'Added EnforceVpn';
    END

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'AccessMode' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [AccessMode] int NOT NULL DEFAULT 0;
        PRINT 'Added AccessMode';
    END

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnableOnboarding' AND Object_ID = Object_ID(N'SystemSettings'))
    BEGIN
        ALTER TABLE [SystemSettings] ADD [EnableOnboarding] bit NOT NULL DEFAULT 0;
        PRINT 'Added EnableOnboarding';
    END

END
ELSE
BEGIN
    PRINT 'Error: SystemSettings table does not exist!';
END

COMMIT;
PRINT 'Schema update completed successfully.';
