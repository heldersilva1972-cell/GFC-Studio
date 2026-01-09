-- Manual Repair Script for SystemSettings Table
-- This script adds missing columns required by the Operations Center

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'AllowServerRestoreOperations')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [AllowServerRestoreOperations] BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'BackupRetentionCount')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [BackupRetentionCount] INT NOT NULL DEFAULT 10;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'BackupStoragePath')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [BackupStoragePath] NVARCHAR(500) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'MaintenanceModeEnabled')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [MaintenanceModeEnabled] BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'PreferredMagicLinkMethod')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [PreferredMagicLinkMethod] NVARCHAR(50) NOT NULL DEFAULT 'Email';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'TwilioAccountSid')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioAccountSid] NVARCHAR(MAX) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'TwilioAuthToken')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioAuthToken] NVARCHAR(MAX) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'TwilioFromNumber')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioFromNumber] NVARCHAR(MAX) NULL;
END

-- Additional columns likely missing based on class definition if it was an older schema
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'BackupMethod')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [BackupMethod] NVARCHAR(MAX) NOT NULL DEFAULT 'External USB';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'LastSuccessfulBackupUtc')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [LastSuccessfulBackupUtc] DATETIME2 NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'LastRestoreTestUtc')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [LastRestoreTestUtc] DATETIME2 NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'BackupFrequencyHours')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [BackupFrequencyHours] INT NOT NULL DEFAULT 24;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'HostingEnvironment')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [HostingEnvironment] NVARCHAR(20) NOT NULL DEFAULT 'Dev';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'TrustedDeviceDurationDays')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [TrustedDeviceDurationDays] INT NOT NULL DEFAULT 30;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'MagicLinkEnabled')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [MagicLinkEnabled] BIT NOT NULL DEFAULT 1;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'EnforceVpn')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [EnforceVpn] BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'AccessMode')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [AccessMode] NVARCHAR(MAX) NOT NULL DEFAULT 'Open';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'EnableOnboarding')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableOnboarding] BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'SafeModeEnabled')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SafeModeEnabled] BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'IdleTimeoutMinutes')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [IdleTimeoutMinutes] INT NOT NULL DEFAULT 20;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'AbsoluteSessionMaxMinutes')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [AbsoluteSessionMaxMinutes] INT NOT NULL DEFAULT 1440;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'PrimaryDomain')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [PrimaryDomain] NVARCHAR(MAX) NULL;
END

-- Ensure at least one row exists
IF NOT EXISTS (SELECT * FROM [dbo].[SystemSettings] WHERE Id = 1)
BEGIN
    INSERT INTO [dbo].[SystemSettings] (Id, LastUpdatedUtc, BackupMethod, BackupFrequencyHours, HostingEnvironment, TrustedDeviceDurationDays, MagicLinkEnabled, EnforceVpn, AccessMode, EnableOnboarding, SafeModeEnabled, IdleTimeoutMinutes, AbsoluteSessionMaxMinutes)
    VALUES (1, GETUTCDATE(), 'External USB', 24, 'Dev', 30, 1, 0, 'Open', 0, 0, 20, 1440);
END
GO
