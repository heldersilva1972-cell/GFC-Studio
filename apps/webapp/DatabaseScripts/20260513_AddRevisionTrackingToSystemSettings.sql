-- FIX: Add missing Revision Tracking columns to SystemSettings
-- This resolves the "Invalid column name 'MobileRevision'..." error.

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'WebappRevision')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [WebappRevision] NVARCHAR(MAX) NULL;
    PRINT 'Added WebappRevision column to SystemSettings';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'MobileRevision')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [MobileRevision] NVARCHAR(MAX) NULL;
    PRINT 'Added MobileRevision column to SystemSettings';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'PosRevision')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [PosRevision] NVARCHAR(MAX) NULL;
    PRINT 'Added PosRevision column to SystemSettings';
END
GO

-- Optional: Initialize with current versions from appsettings
UPDATE [dbo].[SystemSettings]
SET [WebappRevision] = '3.8.72',
    [MobileRevision] = '2.4.26',
    [PosRevision] = '2.38.71'
WHERE [Id] = 1 AND ([WebappRevision] IS NULL OR [WebappRevision] = '');
GO
