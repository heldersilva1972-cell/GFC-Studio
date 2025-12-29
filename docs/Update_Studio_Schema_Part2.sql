-- Script to update GFC Database Schema - Part 2
-- Adds the StudioSettings table required for Global Theme persistence.

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[StudioSettings]') AND type in (N'U'))
BEGIN
    PRINT 'Creating StudioSettings table...'
    CREATE TABLE [StudioSettings] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [SettingKey] NVARCHAR(100) NOT NULL,
        [SettingValue] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [SettingType] NVARCHAR(50) NOT NULL DEFAULT 'General',
        [Description] NVARCHAR(500) NULL,
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System'
    );
    
    CREATE UNIQUE INDEX [IX_StudioSettings_SettingKey] ON [StudioSettings]([SettingKey]);
    
    PRINT 'StudioSettings table created successfully.';
END
ELSE
BEGIN
    PRINT 'StudioSettings table already exists.';
END
GO
