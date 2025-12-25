
-- 1. Update StudioPages table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'IsPublished')
BEGIN
    ALTER TABLE [StudioPages] ADD [IsPublished] bit NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = 'Status')
BEGIN
    ALTER TABLE [StudioPages] ADD [Status] nvarchar(20) NOT NULL DEFAULT 'Draft';
END
GO

-- 2. Update StudioSections table
-- Ensure Data column exists (critical for JSON properties)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'Data')
BEGIN
    ALTER TABLE [StudioSections] ADD [Data] nvarchar(max) NOT NULL DEFAULT '{}';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'ComponentType')
BEGIN
    ALTER TABLE [StudioSections] ADD [ComponentType] nvarchar(100) NOT NULL DEFAULT 'TextBlock';
END
GO

-- 3. Update StudioDrafts table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = 'IsPublished')
BEGIN
    ALTER TABLE [StudioDrafts] ADD [IsPublished] bit NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = 'Version')
BEGIN
    ALTER TABLE [StudioDrafts] ADD [Version] int NOT NULL DEFAULT 1;
END
GO

-- 4. Create StudioSettings table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[StudioSettings]') AND type in (N'U'))
BEGIN
    CREATE TABLE [StudioSettings] (
        [Id] int NOT NULL IDENTITY,
        [SettingKey] nvarchar(100) NOT NULL,
        [SettingValue] nvarchar(max) NULL,
        [Description] nvarchar(500) NULL,
        [LastUpdated] datetime2 NOT NULL,
        [UpdatedBy] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_StudioSettings] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_StudioSettings_SettingKey] ON [StudioSettings] ([SettingKey]);
END
GO

-- 5. Create new Tables for Media/Forms/SEO if they are missing (from GfcDbContext updates)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MediaAssets]') AND type in (N'U'))
BEGIN
    CREATE TABLE [MediaAssets] (
        [Id] int NOT NULL IDENTITY,
        [FileName] nvarchar(255) NOT NULL,
        [ContentType] nvarchar(100) NOT NULL,
        [Size] bigint NOT NULL,
        [Url] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UploadedBy] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_MediaAssets] PRIMARY KEY ([Id])
    );
END
GO

-- Verify StudioPages columns again to be safe
SELECT TOP 1 * FROM [StudioPages];
