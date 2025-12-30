USE [ClubMembership]
GO

PRINT 'Starting COMPREHENSIVE Studio Database Synchronization (Aligned with GfcDbContext)...';

-- 1. Pages Table (Mapped from StudioPage)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pages')
BEGIN
    CREATE TABLE [dbo].[Pages] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Title] NVARCHAR(200) NOT NULL DEFAULT 'New Page',
        [Folder] NVARCHAR(500) NULL DEFAULT '/',
        [Slug] NVARCHAR(200) NOT NULL DEFAULT '',
        [MetaTitle] NVARCHAR(200) NULL,
        [MetaDescription] NVARCHAR(500) NULL,
        [OgImage] NVARCHAR(500) NULL,
        [Status] NVARCHAR(20) NOT NULL DEFAULT 'Draft',
        [PublishedAt] DATETIME2 NULL,
        [PublishedBy] NVARCHAR(100) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        [IsDeleted] BIT NOT NULL DEFAULT 0,
        [DeletedAt] DATETIME2 NULL,
        [DeletedBy] NVARCHAR(100) NULL
    );
    PRINT '✓ Created [Pages] table';
END
ELSE
BEGIN
    PRINT 'Checking [Pages] columns...';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'Folder')
        ALTER TABLE [dbo].[Pages] ADD [Folder] NVARCHAR(500) NULL DEFAULT '/';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'Slug')
        ALTER TABLE [dbo].[Pages] ADD [Slug] NVARCHAR(200) NOT NULL DEFAULT '';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'Status')
        ALTER TABLE [dbo].[Pages] ADD [Status] NVARCHAR(20) NOT NULL DEFAULT 'Draft';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'CreatedAt')
        ALTER TABLE [dbo].[Pages] ADD [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'CreatedBy')
        ALTER TABLE [dbo].[Pages] ADD [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'UpdatedAt')
        ALTER TABLE [dbo].[Pages] ADD [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'UpdatedBy')
        ALTER TABLE [dbo].[Pages] ADD [UpdatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'IsDeleted')
        ALTER TABLE [dbo].[Pages] ADD [IsDeleted] BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'IX_Pages_Slug')
    CREATE UNIQUE INDEX IX_Pages_Slug ON [dbo].[Pages]([Slug]);
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'IX_Pages_Status')
    CREATE INDEX IX_Pages_Status ON [dbo].[Pages]([Status]);
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'IX_Pages_IsDeleted')
    CREATE INDEX IX_Pages_IsDeleted ON [dbo].[Pages]([IsDeleted]);

GO

-- 2. Sections Table (Mapped from StudioSection)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sections')
BEGIN
    CREATE TABLE [dbo].[Sections] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [PageId] INT NOT NULL,
        [ComponentType] NVARCHAR(100) NOT NULL DEFAULT 'TextBlock',
        [OrderIndex] INT NOT NULL DEFAULT 0,
        [ContentJson] NVARCHAR(MAX) NOT NULL DEFAULT '{}',
        [AnimationJson] NVARCHAR(MAX) NULL,
        [Styles] NVARCHAR(MAX) NULL DEFAULT '{}',
        [InteractionJson] NVARCHAR(MAX) NULL DEFAULT '[]',
        [DataBindingJson] NVARCHAR(MAX) NULL DEFAULT '{}',
        [IsVisible] BIT NOT NULL DEFAULT 1,
        [VisibleOnDesktop] BIT NOT NULL DEFAULT 1,
        [VisibleOnTablet] BIT NOT NULL DEFAULT 1,
        [VisibleOnMobile] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        CONSTRAINT FK_Sections_Pages FOREIGN KEY (PageId) REFERENCES Pages(Id) ON DELETE CASCADE
    );
    PRINT '✓ Created [Sections] table';
END
ELSE
BEGIN
    PRINT 'Checking [Sections] columns...';
    -- Rename legacy columns only if the OLD exists AND the NEW does NOT exist
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'StudioPageId')
       AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'PageId')
        EXEC sp_rename 'Sections.StudioPageId', 'PageId', 'COLUMN';

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'Data')
       AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'ContentJson')
        EXEC sp_rename 'Sections.Data', 'ContentJson', 'COLUMN';

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'AnimationSettingsJson')
       AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'AnimationJson')
        EXEC sp_rename 'Sections.AnimationSettingsJson', 'AnimationJson', 'COLUMN';

    -- Ensure required columns exist
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'PageId')
        ALTER TABLE [dbo].[Sections] ADD [PageId] INT NOT NULL DEFAULT 0;
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'ContentJson')
        ALTER TABLE [dbo].[Sections] ADD [ContentJson] NVARCHAR(MAX) NOT NULL DEFAULT '{}';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'AnimationJson')
        ALTER TABLE [dbo].[Sections] ADD [AnimationJson] NVARCHAR(MAX) NULL;
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'Styles')
        ALTER TABLE [dbo].[Sections] ADD [Styles] NVARCHAR(MAX) NULL DEFAULT '{}';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'InteractionJson')
        ALTER TABLE [dbo].[Sections] ADD [InteractionJson] NVARCHAR(MAX) NULL DEFAULT '[]';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'DataBindingJson')
        ALTER TABLE [dbo].[Sections] ADD [DataBindingJson] NVARCHAR(MAX) NULL DEFAULT '{}';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'IX_Sections_OrderIndex')
    CREATE INDEX IX_Sections_OrderIndex ON [dbo].[Sections]([OrderIndex]);
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND name = 'IX_Sections_ComponentType')
    CREATE INDEX IX_Sections_ComponentType ON [dbo].[Sections]([ComponentType]);

GO

-- 3. Drafts Table (Mapped from StudioDraft)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Drafts')
BEGIN
    CREATE TABLE [dbo].[Drafts] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [StudioPageId] INT NOT NULL,
        [ContentSnapshotJson] NVARCHAR(MAX) NOT NULL DEFAULT '[]',
        [ChangeDescription] NVARCHAR(500) NULL,
        [Version] INT NOT NULL DEFAULT 1,
        [IsPublished] BIT NOT NULL DEFAULT 0,
        [PublishedAt] DATETIME2 NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        CONSTRAINT FK_Drafts_Pages FOREIGN KEY (StudioPageId) REFERENCES Pages(Id) ON DELETE CASCADE
    );
    PRINT '✓ Created [Drafts] table';
END
ELSE
BEGIN
    PRINT 'Checking [Drafts] columns...';
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Drafts]') AND name = 'Version')
        ALTER TABLE [dbo].[Drafts] ADD [Version] INT NOT NULL DEFAULT 1;
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Drafts]') AND name = 'IX_Drafts_Page_Version')
    CREATE INDEX IX_Drafts_Page_Version ON [dbo].[Drafts]([StudioPageId], [Version] DESC);
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Drafts]') AND name = 'IX_Drafts_CreatedAt')
    CREATE INDEX IX_Drafts_CreatedAt ON [dbo].[Drafts]([CreatedAt] DESC);

GO

-- 4. Templates Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Templates')
BEGIN
    CREATE TABLE [dbo].[Templates] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [Category] NVARCHAR(50) NOT NULL,
        [ContentJson] NVARCHAR(MAX) NOT NULL,
        [ThumbnailUrl] NVARCHAR(MAX) NULL,
        [UsageCount] INT NOT NULL DEFAULT 0,
        [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
    PRINT '✓ Created [Templates] table';
END
GO

-- 5. StudioSettings Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioSettings')
BEGIN
    CREATE TABLE [dbo].[StudioSettings] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [SettingKey] NVARCHAR(100) NOT NULL,
        [SettingValue] NVARCHAR(MAX) NULL,
        [SettingType] NVARCHAR(50) NOT NULL DEFAULT 'General',
        [Description] NVARCHAR(500) NULL,
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System'
    );
    CREATE UNIQUE INDEX IX_StudioSettings_SettingKey ON StudioSettings(SettingKey);
    PRINT '✓ Created [StudioSettings] table';
END
ELSE
BEGIN
    PRINT 'Checking [StudioSettings] columns...';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StudioSettings]') AND name = 'SettingType')
        ALTER TABLE [dbo].[StudioSettings] ADD [SettingType] NVARCHAR(50) NOT NULL DEFAULT 'General';

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StudioSettings]') AND name = 'LastUpdated')
       AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StudioSettings]') AND name = 'UpdatedAt')
        EXEC sp_rename 'StudioSettings.LastUpdated', 'UpdatedAt', 'COLUMN';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StudioSettings]') AND name = 'UpdatedAt')
        ALTER TABLE [dbo].[StudioSettings] ADD [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
END
GO

PRINT '✓ COMPREHENSIVE Studio Database Synchronization Complete.';
GO
