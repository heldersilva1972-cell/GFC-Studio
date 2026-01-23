-- Comprehensive Schema for GFC Studio V2 Tables
-- Use this if the tables do not exist at all.

-- 1. Create Pages Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Pages]') AND type in (N'U'))
BEGIN
    CREATE TABLE [Pages] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Title] NVARCHAR(100) NOT NULL,
        [Slug] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(MAX) NULL,
        [Status] INT NOT NULL DEFAULT 0, -- 0: Draft, 1: Published
        [IsDeleted] BIT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NULL,
        [UpdatedBy] NVARCHAR(100) NULL
    );
    CREATE UNIQUE INDEX [IX_Pages_Slug] ON [Pages]([Slug]);
END
GO

-- 2. Create Sections Table (which mapped to StudioSection entities)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sections]') AND type in (N'U'))
BEGIN
    CREATE TABLE [Sections] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [PageId] INT NOT NULL,
        [ComponentType] NVARCHAR(100) NOT NULL DEFAULT 'TextBlock',
        [OrderIndex] INT NOT NULL DEFAULT 0,
        [ContentJson] NVARCHAR(MAX) NOT NULL DEFAULT '{}',
        [AnimationJson] NVARCHAR(MAX) NULL,
        [Styles] NVARCHAR(MAX) NULL DEFAULT '{}', -- The column we need
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        [ClientId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        CONSTRAINT [FK_Sections_Pages_PageId] FOREIGN KEY ([PageId]) REFERENCES [Pages] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_Sections_PageId] ON [Sections]([PageId]);
END
ELSE
BEGIN
    -- If table exists but missing Styles column (the original error fix)
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Sections]') AND name = 'Styles')
    BEGIN
        ALTER TABLE [Sections] ADD [Styles] NVARCHAR(MAX) NULL DEFAULT '{}';
    END
    -- If table exists but missing AnimationJson column
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Sections]') AND name = 'AnimationJson')
    BEGIN
        ALTER TABLE [Sections] ADD [AnimationJson] NVARCHAR(MAX) NULL;
    END
END
GO

-- 3. Create Drafts Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Drafts]') AND type in (N'U'))
BEGIN
    CREATE TABLE [Drafts] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [StudioPageId] INT NOT NULL,
        [Version] INT NOT NULL,
        [ContentSnapshotJson] NVARCHAR(MAX) NULL,
        [ChangeDescription] NVARCHAR(MAX) NULL,
        [IsPublished] BIT NOT NULL DEFAULT 0,
        [PublishedAt] DATETIME2 NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NULL,
        CONSTRAINT [FK_Drafts_Pages_StudioPageId] FOREIGN KEY ([StudioPageId]) REFERENCES [Pages] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_Drafts_StudioPageId] ON [Drafts]([StudioPageId]);
END
GO

-- 4. Create StudioLocks Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[StudioLocks]') AND type in (N'U'))
BEGIN
    CREATE TABLE [StudioLocks] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [PageId] INT NOT NULL,
        [LockedBy] NVARCHAR(100) NOT NULL,
        [LockedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [FK_StudioLocks_Pages_PageId] FOREIGN KEY ([PageId]) REFERENCES [Pages] ([Id]) ON DELETE CASCADE
    );
END
GO

-- 5. Create Templates Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Templates]') AND type in (N'U'))
BEGIN
    CREATE TABLE [Templates] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [Category] NVARCHAR(50) NOT NULL,
        [ContentJson] NVARCHAR(MAX) NOT NULL,
        [UsageCount] INT NOT NULL DEFAULT 0,
        [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

PRINT 'Full Studio V2 Schema Ensure Script Completed.';
