-- Manually generated script based on 20251227000000_AddStudioV2Schema.cs

BEGIN TRANSACTION;

-- Drop foreign keys first to avoid conflicts
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_StudioSections_StudioPages_StudioPageId')
BEGIN
    ALTER TABLE [StudioSections] DROP CONSTRAINT [FK_StudioSections_StudioPages_StudioPageId];
END;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Sections_Pages_PageId')
BEGIN
    ALTER TABLE [Sections] DROP CONSTRAINT [FK_Sections_Pages_PageId];
END;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Drafts_Pages_PageId')
BEGIN
    ALTER TABLE [Drafts] DROP CONSTRAINT [FK_Drafts_Pages_PageId];
END;

-- Drop existing tables if they exist
DROP TABLE IF EXISTS [StudioDrafts];
DROP TABLE IF EXISTS [StudioSections];
DROP TABLE IF EXISTS [StudioTemplates];
DROP TABLE IF EXISTS [StudioPages];

DROP TABLE IF EXISTS [Drafts];
DROP TABLE IF EXISTS [Sections];
DROP TABLE IF EXISTS [Templates];
DROP TABLE IF EXISTS [Pages];

-- Create Pages table
CREATE TABLE [Pages] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Slug] nvarchar(200) NOT NULL,
    [MetaTitle] nvarchar(200) NULL,
    [MetaDescription] nvarchar(500) NULL,
    [OgImage] nvarchar(500) NULL,
    [Status] nvarchar(20) NOT NULL DEFAULT N'Draft',
    [PublishedAt] datetime2 NULL,
    [PublishedBy] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [CreatedBy] nvarchar(100) NOT NULL,
    [UpdatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedBy] nvarchar(100) NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(100) NULL,
    CONSTRAINT [PK_Pages] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Pages_Slug] ON [Pages] ([Slug]);
CREATE INDEX [IX_Pages_Status] ON [Pages] ([Status]);
CREATE INDEX [IX_Pages_IsDeleted] ON [Pages] ([IsDeleted]);

-- Create Sections table
CREATE TABLE [Sections] (
    [Id] int NOT NULL IDENTITY,
    [PageId] int NOT NULL,
    [ComponentType] nvarchar(100) NOT NULL,
    [OrderIndex] int NOT NULL,
    [ContentJson] nvarchar(max) NOT NULL,
    [StylesJson] nvarchar(max) NULL,
    [AnimationJson] nvarchar(max) NULL,
    [ResponsiveJson] nvarchar(max) NULL,
    [IsVisible] bit NOT NULL DEFAULT 1,
    [VisibleOnDesktop] bit NOT NULL DEFAULT 1,
    [VisibleOnTablet] bit NOT NULL DEFAULT 1,
    [VisibleOnMobile] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [CreatedBy] nvarchar(100) NOT NULL,
    [UpdatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Sections] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Sections_Pages_PageId] FOREIGN KEY ([PageId]) REFERENCES [Pages] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Sections_PageId] ON [Sections] ([PageId]);
CREATE INDEX [IX_Sections_OrderIndex] ON [Sections] ([OrderIndex]);
CREATE INDEX [IX_Sections_ComponentType] ON [Sections] ([ComponentType]);

-- Create Drafts table
CREATE TABLE [Drafts] (
    [Id] int NOT NULL IDENTITY,
    [PageId] int NOT NULL,
    [Version] int NOT NULL,
    [ContentJson] nvarchar(max) NOT NULL,
    [ChangeDescription] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [CreatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Drafts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Drafts_Pages_PageId] FOREIGN KEY ([PageId]) REFERENCES [Pages] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Drafts_PageId_Version] ON [Drafts] ([PageId], [Version] DESC);
CREATE INDEX [IX_Drafts_CreatedAt] ON [Drafts] ([CreatedAt] DESC);

-- Create Templates table
CREATE TABLE [Templates] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [Category] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [ThumbnailUrl] nvarchar(500) NULL,
    [ComponentType] nvarchar(100) NOT NULL,
    [ContentJson] nvarchar(max) NOT NULL,
    [IsPublic] bit NOT NULL DEFAULT 0,
    [UsageCount] int NOT NULL DEFAULT 0,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [CreatedBy] nvarchar(100) NOT NULL,
    [UpdatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Templates] PRIMARY KEY ([Id])
);

CREATE INDEX [IX_Templates_Category] ON [Templates] ([Category]);
CREATE INDEX [IX_Templates_CreatedBy] ON [Templates] ([CreatedBy]);
CREATE INDEX [IX_Templates_UsageCount] ON [Templates] ([UsageCount] DESC);

IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251227000000_AddStudioV2Schema')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251227000000_AddStudioV2Schema', N'8.0.0');
END;

COMMIT;
