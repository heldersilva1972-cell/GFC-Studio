-- LIQUOR & MEDIA SCHEMA AUTO-FIX --
-- Aligns database with GFC.Core.Models for Liquor and Media systems

USE [ClubMembership];
GO

PRINT 'Starting Liquor & Media System Schema Alignment...';

-- 1. FIX LIQUOR ITEMS
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LiquorItems')
BEGIN
    CREATE TABLE [dbo].[LiquorItems] (
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](200) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [UpcCode] [nvarchar](100) NULL,
        [BottleSize] [nvarchar](50) NULL,
        [Category] [nvarchar](100) NULL,
        [ImageUrl] [nvarchar](max) NULL,
        [CurrentStock] [int] NOT NULL DEFAULT 0,
        [MinStockLimit] [int] NOT NULL DEFAULT 2,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_LiquorItems] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '✓ Created LiquorItems table';
END
ELSE
BEGIN
    PRINT 'Checking LiquorItems columns...';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[LiquorItems]') AND name = 'ImageUrl')
        ALTER TABLE [dbo].[LiquorItems] ADD [ImageUrl] [nvarchar](max) NULL;
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[LiquorItems]') AND name = 'BottleSize')
        ALTER TABLE [dbo].[LiquorItems] ADD [BottleSize] [nvarchar](50) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[LiquorItems]') AND name = 'MinStockLimit')
        ALTER TABLE [dbo].[LiquorItems] ADD [MinStockLimit] [int] NOT NULL DEFAULT 2;
        
    PRINT '✓ Verified LiquorItems columns';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LiquorItems_UpcCode' AND object_id = OBJECT_ID('LiquorItems'))
BEGIN
    CREATE UNIQUE INDEX [IX_LiquorItems_UpcCode] ON [dbo].[LiquorItems] ([UpcCode]) WHERE [UpcCode] IS NOT NULL;
    PRINT '✓ Created unique index on UpcCode';
END

-- 2. FIX LIQUOR TRANSACTIONS
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LiquorTransactions')
BEGIN
    CREATE TABLE [dbo].[LiquorTransactions] (
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ItemId] [int] NOT NULL,
        [UserId] [int] NOT NULL,
        [ChangeAmount] [int] NOT NULL,
        [TransactionType] [nvarchar](50) NOT NULL,
        [Timestamp] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [Notes] [nvarchar](500) NULL,
        CONSTRAINT [PK_LiquorTransactions] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_LiquorTransactions_LiquorItems] FOREIGN KEY([ItemId]) REFERENCES [dbo].[LiquorItems] ([Id]) ON DELETE CASCADE
    );
    PRINT '✓ Created LiquorTransactions table';
END

-- 3. FIX MEDIA ASSETS
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
        IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'FolderId')
            EXEC sp_rename 'MediaAssets.FolderId', 'AssetFolderId', 'COLUMN';
        ELSE
            ALTER TABLE [dbo].[MediaAssets] ADD [AssetFolderId] INT NULL;
    END

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'RequiredRole')
        ALTER TABLE [dbo].[MediaAssets] ADD [RequiredRole] NVARCHAR(100) NULL;

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'FilePath')
        ALTER TABLE [dbo].[MediaAssets] DROP COLUMN [FilePath];

    PRINT '✓ Verified MediaAssets columns';
END

-- 4. FIX MEDIA RENDITIONS
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

    -- Renames
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'AssetId')
       AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'MediaAssetId')
        EXEC sp_rename 'MediaRenditions.AssetId', 'MediaAssetId', 'COLUMN';

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'Name')
       AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'RenditionType')
        EXEC sp_rename 'MediaRenditions.Name', 'RenditionType', 'COLUMN';

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'FilePath')
       AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'Url')
        EXEC sp_rename 'MediaRenditions.FilePath', 'Url', 'COLUMN';

    -- Additions
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'MediaAssetId')
        ALTER TABLE [dbo].[MediaRenditions] ADD [MediaAssetId] INT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'RenditionType')
        ALTER TABLE [dbo].[MediaRenditions] ADD [RenditionType] NVARCHAR(50) NOT NULL DEFAULT 'unknown';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'Url')
        ALTER TABLE [dbo].[MediaRenditions] ADD [Url] NVARCHAR(1024) NOT NULL DEFAULT '';

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaRenditions]') AND name = 'FileSize')
        ALTER TABLE [dbo].[MediaRenditions] DROP COLUMN [FileSize];

    PRINT '✓ Verified MediaRenditions columns';
END

PRINT 'Liquor & Media Schema Alignment Complete.';
GO
