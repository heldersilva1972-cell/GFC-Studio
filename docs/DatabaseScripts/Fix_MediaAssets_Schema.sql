-- =============================================
-- FIX MEDIA ASSETS & RENDITIONS SCHEMA
-- Aligns database with GFC.Core.Models.MediaAsset and MediaRendition
-- =============================================

USE [ClubMembership];
GO

PRINT 'Aligning MediaAssets and MediaRenditions with C# Models...';

-- 1. Fix MediaAssets Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MediaAssets')
BEGIN
    CREATE TABLE [dbo].[MediaAssets] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FileName] NVARCHAR(255) NOT NULL,
        [StoredFileName] NVARCHAR(255) NOT NULL,
        [ContentType] NVARCHAR(100) NOT NULL,
        [FileSize] BIGINT NOT NULL DEFAULT 0,
        [UploadedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [Tag] NVARCHAR(100) NULL,
        [UploadedBy] NVARCHAR(MAX) NULL,
        [Usage] NVARCHAR(MAX) NULL,
        [AssetFolderId] INT NULL,
        [RequiredRole] NVARCHAR(100) NULL
    );
    PRINT '✓ Created MediaAssets table';
END
ELSE
BEGIN
    PRINT 'Checking MediaAssets columns...';
    
    -- Add missing columns
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'StoredFileName')
        ALTER TABLE [dbo].[MediaAssets] ADD [StoredFileName] NVARCHAR(255) NOT NULL DEFAULT '';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'Tag')
        ALTER TABLE [dbo].[MediaAssets] ADD [Tag] NVARCHAR(100) NULL;
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'UploadedBy')
        ALTER TABLE [dbo].[MediaAssets] ADD [UploadedBy] NVARCHAR(MAX) NULL;
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'Usage')
        ALTER TABLE [dbo].[MediaAssets] ADD [Usage] NVARCHAR(MAX) NULL;
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'AssetFolderId')
    BEGIN
        -- Try to rename from FolderId if it exists
        IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'FolderId')
            EXEC sp_rename 'MediaAssets.FolderId', 'AssetFolderId', 'COLUMN';
        ELSE
            ALTER TABLE [dbo].[MediaAssets] ADD [AssetFolderId] INT NULL;
    END

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'RequiredRole')
        ALTER TABLE [dbo].[MediaAssets] ADD [RequiredRole] NVARCHAR(100) NULL;

    -- Remote FilePath which is [NotMapped] in C# model
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'FilePath')
        ALTER TABLE [dbo].[MediaAssets] DROP COLUMN [FilePath];

    PRINT '✓ Verified MediaAssets columns';
END

-- 2. Fix MediaRenditions Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MediaRenditions')
BEGIN
    CREATE TABLE [dbo].[MediaRenditions] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [MediaAssetId] INT NOT NULL,
        [RenditionType] NVARCHAR(50) NOT NULL,
        [Url] NVARCHAR(1024) NOT NULL,
        [Width] INT NOT NULL DEFAULT 0,
        [Height] INT NOT NULL DEFAULT 0,
        CONSTRAINT FK_MediaRenditions_MediaAssets FOREIGN KEY (MediaAssetId) REFERENCES MediaAssets(Id) ON DELETE CASCADE
    );
    PRINT '✓ Created MediaRenditions table';
END
ELSE
BEGIN
    PRINT 'Checking MediaRenditions columns...';

    -- Rename AssetId -> MediaAssetId
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'AssetId')
       AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'MediaAssetId')
        EXEC sp_rename 'MediaRenditions.AssetId', 'MediaAssetId', 'COLUMN';

    -- Rename Name -> RenditionType
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'Name')
       AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'RenditionType')
        EXEC sp_rename 'MediaRenditions.Name', 'RenditionType', 'COLUMN';

    -- Rename FilePath -> Url
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'FilePath')
       AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'Url')
        EXEC sp_rename 'MediaRenditions.FilePath', 'Url', 'COLUMN';

    -- Add missing columns
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'MediaAssetId')
        ALTER TABLE [dbo].[MediaRenditions] ADD [MediaAssetId] INT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'RenditionType')
        ALTER TABLE [dbo].[MediaRenditions] ADD [RenditionType] NVARCHAR(50) NOT NULL DEFAULT 'unknown';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'Url')
        ALTER TABLE [dbo].[MediaRenditions] ADD [Url] NVARCHAR(1024) NOT NULL DEFAULT '';

    -- Remove FileSize which is NOT in the model
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'FileSize')
        ALTER TABLE [dbo].[MediaRenditions] DROP COLUMN [FileSize];

    PRINT '✓ Verified MediaRenditions columns';
END
GO

PRINT 'Media Schema Alignment Complete.';
GO
