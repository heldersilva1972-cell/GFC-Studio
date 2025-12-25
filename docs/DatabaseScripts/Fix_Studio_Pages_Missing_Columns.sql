
-- Fix missing columns in StudioPages table
-- Based on error: Invalid column name 'CreatedAt', 'CreatedBy', 'DeletedAt', etc.

-- 1. Audit Columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'CreatedAt')
BEGIN
    ALTER TABLE [StudioPages] ADD [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE();
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'CreatedBy')
BEGIN
    ALTER TABLE [StudioPages] ADD [CreatedBy] nvarchar(100) NOT NULL DEFAULT 'System';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'UpdatedAt')
BEGIN
    ALTER TABLE [StudioPages] ADD [UpdatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE();
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'UpdatedBy')
BEGIN
    ALTER TABLE [StudioPages] ADD [UpdatedBy] nvarchar(100) NOT NULL DEFAULT 'System';
END
GO

-- 2. Soft Delete Columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE [StudioPages] ADD [IsDeleted] bit NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'DeletedAt')
BEGIN
    ALTER TABLE [StudioPages] ADD [DeletedAt] datetime2 NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'DeletedBy')
BEGIN
    ALTER TABLE [StudioPages] ADD [DeletedBy] nvarchar(100) NULL;
END
GO

-- 3. SEO / Metadata Columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'MetaTitle')
BEGIN
    ALTER TABLE [StudioPages] ADD [MetaTitle] nvarchar(200) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'MetaDescription')
BEGIN
    ALTER TABLE [StudioPages] ADD [MetaDescription] nvarchar(500) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'OgImage')
BEGIN
    ALTER TABLE [StudioPages] ADD [OgImage] nvarchar(500) NULL;
END
GO

-- 4. Publication Columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'PublishedAt')
BEGIN
    ALTER TABLE [StudioPages] ADD [PublishedAt] datetime2 NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'PublishedBy')
BEGIN
    ALTER TABLE [StudioPages] ADD [PublishedBy] nvarchar(100) NULL;
END
GO

-- Verify columns in StudioPages
SELECT TOP 1 * FROM [StudioPages];
