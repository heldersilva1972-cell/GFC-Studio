USE [ClubMembership];
GO

-- 1. Create Base Tables if missing
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioPages')
BEGIN
    CREATE TABLE StudioPages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(MAX) NOT NULL,
        Content NVARCHAR(MAX) NULL,
        IsPublished BIT NOT NULL DEFAULT 0
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioSections')
BEGIN
    CREATE TABLE StudioSections (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ClientId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Title NVARCHAR(MAX) NOT NULL,
        Content NVARCHAR(MAX) NULL,
        PageIndex INT NOT NULL DEFAULT 0,
        AnimationSettings NVARCHAR(MAX) NULL,
        StudioPageId INT NOT NULL,
        CONSTRAINT FK_StudioSections_StudioPages FOREIGN KEY (StudioPageId) 
            REFERENCES StudioPages(Id) ON DELETE CASCADE
    );
END

-- 2. Ensure Slug column exists
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = N'Slug')
BEGIN
    ALTER TABLE [StudioPages] ADD [Slug] NVARCHAR(255) NOT NULL DEFAULT '';
END
GO

-- 3. CLEANUP: Fix duplicate empty slugs before creating the Unique Index
-- This prevents the "duplicate key value" error in SSMS
UPDATE StudioPages 
SET Slug = 'page-' + CAST(Id AS NVARCHAR(10))
WHERE Slug = '' OR Slug IS NULL;
GO

-- 4. Create the Unique Index
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = N'IX_StudioPages_Slug')
BEGIN
    CREATE UNIQUE INDEX [IX_StudioPages_Slug] ON [StudioPages] ([Slug]);
END
GO

-- 5. StudioDrafts & Versioning Updates
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioDrafts')
BEGIN
    CREATE TABLE StudioDrafts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        PageId INT NOT NULL,
        ContentJson NVARCHAR(MAX) NULL,
        Version INT NOT NULL DEFAULT 1,
        CreatedBy NVARCHAR(MAX) NOT NULL DEFAULT 'System',
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
ELSE
BEGIN
    -- Rename legacy column if it exists in existing table
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = N'ContentSnapshotJson')
        EXEC sp_rename 'StudioDrafts.ContentSnapshotJson', 'ContentJson', 'COLUMN';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = N'Version')
        ALTER TABLE [StudioDrafts] ADD [Version] INT NOT NULL DEFAULT 1;
END

-- 6. Add LastPublishedAt to Pages
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = N'LastPublishedAt')
    ALTER TABLE [StudioPages] ADD [LastPublishedAt] DATETIME2 NULL;

PRINT 'Master Studio Schema successfully synchronized.';
