USE [ClubMembership];
GO

-- 1. Update StudioPages table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = N'LastPublishedAt')
BEGIN
    ALTER TABLE [StudioPages] ADD [LastPublishedAt] DATETIME2 NULL;
END
GO

-- 2. Update StudioDrafts table
-- Rename ContentSnapshotJson to ContentJson if it exists
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = N'ContentSnapshotJson')
BEGIN
    EXEC sp_rename 'StudioDrafts.ContentSnapshotJson', 'ContentJson', 'COLUMN';
END
GO

-- Add ContentJson if it doesn't exist (in case rename failed or it was missing)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = N'ContentJson')
BEGIN
    ALTER TABLE [StudioDrafts] ADD [ContentJson] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = N'Version')
BEGIN
    ALTER TABLE [StudioDrafts] ADD [Version] INT NOT NULL DEFAULT 0;
END
GO
