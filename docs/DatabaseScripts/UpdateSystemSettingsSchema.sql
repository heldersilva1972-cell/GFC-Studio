USE ClubMembership;
GO
DECLARE @tableName NVARCHAR(128) = N'[SystemSettings]';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(@tableName) AND name = 'BackupStoragePath')
    ALTER TABLE [SystemSettings] ADD [BackupStoragePath] NVARCHAR(500) NOT NULL DEFAULT 'C:\GFC_Backups\Sql\';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(@tableName) AND name = 'BackupRetentionCount')
    ALTER TABLE [SystemSettings] ADD [BackupRetentionCount] INT NOT NULL DEFAULT 10;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(@tableName) AND name = 'AllowServerRestoreOperations')
    ALTER TABLE [SystemSettings] ADD [AllowServerRestoreOperations] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(@tableName) AND name = 'MaintenanceModeEnabled')
    ALTER TABLE [SystemSettings] ADD [MaintenanceModeEnabled] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(@tableName) AND name = 'TwilioAccountSid')
    ALTER TABLE [SystemSettings] ADD [TwilioAccountSid] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(@tableName) AND name = 'TwilioAuthToken')
    ALTER TABLE [SystemSettings] ADD [TwilioAuthToken] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(@tableName) AND name = 'TwilioFromNumber')
    ALTER TABLE [SystemSettings] ADD [TwilioFromNumber] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(@tableName) AND name = 'PreferredMagicLinkMethod')
    ALTER TABLE [SystemSettings] ADD [PreferredMagicLinkMethod] NVARCHAR(MAX) NOT NULL DEFAULT 'Email';
GO

-- Reset sync state to force re-evaluation
DELETE FROM ControllerLastIndexes;
GO
