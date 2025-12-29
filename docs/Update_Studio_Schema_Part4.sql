-- Script to update GFC Database Schema - Part 4
-- Adds CMS (Content Management System) tables for Studio V2.

-- 1. StudioCollections (e.g., "Blog Posts", "Team Members")
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[StudioCollections]') AND type in (N'U'))
BEGIN
    CREATE TABLE [StudioCollections] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [Slug] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
    CREATE UNIQUE INDEX [IX_StudioCollections_Slug] ON [StudioCollections]([Slug]);
END
GO

-- 2. StudioCollectionFields (e.g., "Title", "Cover Image", "Content")
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[StudioCollectionFields]') AND type in (N'U'))
BEGIN
    CREATE TABLE [StudioCollectionFields] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [CollectionId] INT NOT NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [Key] NVARCHAR(100) NOT NULL, -- Logical key for binding
        [Type] NVARCHAR(50) NOT NULL, -- Text, RichText, Image, Date, Number
        [IsRequired] BIT NOT NULL DEFAULT 0,
        CONSTRAINT [FK_StudioCollectionFields_StudioCollections_CollectionId] FOREIGN KEY ([CollectionId]) REFERENCES [StudioCollections] ([Id]) ON DELETE CASCADE
    );
END
GO

-- 3. StudioCollectionItems (The actual data rows)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[StudioCollectionItems]') AND type in (N'U'))
BEGIN
    CREATE TABLE [StudioCollectionItems] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [CollectionId] INT NOT NULL,
        [DataJson] NVARCHAR(MAX) NOT NULL DEFAULT '{}', -- Flexible storage for field values
        [IsPublished] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [FK_StudioCollectionItems_StudioCollections_CollectionId] FOREIGN KEY ([CollectionId]) REFERENCES [StudioCollections] ([Id]) ON DELETE CASCADE
    );
END
GO

PRINT 'CMS Schema update completed successfully.';
