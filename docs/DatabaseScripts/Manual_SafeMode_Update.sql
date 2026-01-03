IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'SafeModeEnabled')
BEGIN
    ALTER TABLE [SystemSettings] ADD [SafeModeEnabled] bit NOT NULL DEFAULT 0;
END
GO
