-- Add Theme and Design columns to WebsiteSettings table
-- This script is idempotent (safe to run multiple times)

USE [ClubMembership]
GO

-- Add PrimaryColor column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PrimaryColor')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [PrimaryColor] NVARCHAR(50) NULL DEFAULT '#0D1B2A';
    PRINT 'Added PrimaryColor column';
END
GO

-- Add SecondaryColor column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SecondaryColor')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [SecondaryColor] NVARCHAR(50) NULL DEFAULT '#FFD700';
    PRINT 'Added SecondaryColor column';
END
GO

-- Add HeadingFont column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'HeadingFont')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [HeadingFont] NVARCHAR(100) NULL DEFAULT 'Outfit';
    PRINT 'Added HeadingFont column';
END
GO

-- Add BodyFont column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'BodyFont')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [BodyFont] NVARCHAR(100) NULL DEFAULT 'Inter';
    PRINT 'Added BodyFont column';
END
GO

-- Add LargeTextMode column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'LargeTextMode')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [LargeTextMode] BIT NULL DEFAULT 0;
    PRINT 'Added LargeTextMode column';
END
GO

-- Add SeoTitle column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoTitle')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoTitle] NVARCHAR(200) NULL;
    PRINT 'Added SeoTitle column';
END
GO

-- Add SeoDescription column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoDescription')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoDescription] NVARCHAR(500) NULL;
    PRINT 'Added SeoDescription column';
END
GO

-- Add SeoKeywords column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoKeywords')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoKeywords] NVARCHAR(500) NULL;
    PRINT 'Added SeoKeywords column';
END
GO

-- Update existing rows with default values if they are NULL
UPDATE [dbo].[WebsiteSettings]
SET 
    [PrimaryColor] = ISNULL([PrimaryColor], '#0D1B2A'),
    [SecondaryColor] = ISNULL([SecondaryColor], '#FFD700'),
    [HeadingFont] = ISNULL([HeadingFont], 'Outfit'),
    [BodyFont] = ISNULL([BodyFont], 'Inter'),
    [LargeTextMode] = ISNULL([LargeTextMode], 0)
WHERE [Id] = 1;
GO

PRINT 'WebsiteSettings theme columns migration completed successfully';
GO
