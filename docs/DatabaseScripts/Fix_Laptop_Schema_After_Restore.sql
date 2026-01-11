-- FIX SCRIPT: Add missing columns to match GFC Studio V2 Codebase
-- TARGET DATABASE: ClubMembership (on .\SQLEXPRESS)

USE ClubMembership;
GO

-- 1. Fix BarSaleEntries (add Shift and AdjustedSaleDate)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BarSaleEntries]') AND name = 'Shift')
BEGIN
    PRINT 'Adding Shift column to BarSaleEntries...';
    ALTER TABLE [dbo].[BarSaleEntries] ADD [Shift] nvarchar(20) NOT NULL DEFAULT 'Day';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BarSaleEntries]') AND name = 'AdjustedSaleDate')
BEGIN
    PRINT 'Adding AdjustedSaleDate column to BarSaleEntries...';
    ALTER TABLE [dbo].[BarSaleEntries] ADD [AdjustedSaleDate] datetime2 NULL;
END
GO

-- 2. Fix SystemSettings (add missing Twilio, NVR, Backup columns)
-- Check and Add Twilio Columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'TwilioAccountSid')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioAccountSid] nvarchar(max) NULL;
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'TwilioAuthToken')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioAuthToken] nvarchar(max) NULL;
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'TwilioFromNumber')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioFromNumber] nvarchar(max) NULL;
END

-- Check and Add NVR Columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrIpAddress')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrIpAddress] nvarchar(max) NULL;
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrUsername')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrUsername] nvarchar(max) NULL;
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrPassword')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrPassword] nvarchar(max) NULL;
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrPort')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrPort] int NOT NULL DEFAULT 80;
END

-- Check and Add Backup/Restore Columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BackupStoragePath')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [BackupStoragePath] nvarchar(max) NULL;
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BackupRetentionCount')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [BackupRetentionCount] int NOT NULL DEFAULT 5;
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'AllowServerRestoreOperations')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [AllowServerRestoreOperations] bit NOT NULL DEFAULT 0;
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaintenanceModeEnabled')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [MaintenanceModeEnabled] bit NOT NULL DEFAULT 0;
END

-- Check and Add MagicLink Columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PreferredMagicLinkMethod')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [PreferredMagicLinkMethod] int NOT NULL DEFAULT 0;
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SystemTimeZoneId')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SystemTimeZoneId] nvarchar(max) NOT NULL DEFAULT 'Eastern Standard Time';
END

PRINT 'Schema update completed successfully. Your Laptop Database is now synced with Codebase V2.';
