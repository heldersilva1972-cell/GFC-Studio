IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'BackupMethod')
BEGIN
    ALTER TABLE [SystemSettings] ADD [BackupMethod] nvarchar(MAX) NOT NULL DEFAULT N'External USB';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'LastSuccessfulBackupUtc')
BEGIN
    ALTER TABLE [SystemSettings] ADD [LastSuccessfulBackupUtc] datetime2 NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'LastRestoreTestUtc')
BEGIN
    ALTER TABLE [SystemSettings] ADD [LastRestoreTestUtc] datetime2 NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'BackupFrequencyHours')
BEGIN
    ALTER TABLE [SystemSettings] ADD [BackupFrequencyHours] int NOT NULL DEFAULT 24;
END
GO
