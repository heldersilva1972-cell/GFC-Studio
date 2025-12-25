USE [ClubMembership];
GO

PRINT 'Starting Studio Database Reconciliation...';

-- 1. StudioPages Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioPages')
BEGIN
    CREATE TABLE StudioPages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(100) NOT NULL,
        Slug NVARCHAR(100) NULL,
        IsPublished BIT NOT NULL DEFAULT 0
    );
    PRINT 'Created StudioPages table.';
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioPages') AND name = 'Slug')
        ALTER TABLE StudioPages ADD Slug NVARCHAR(100) NULL;
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioPages') AND name = 'IsPublished')
        ALTER TABLE StudioPages ADD IsPublished BIT NOT NULL DEFAULT 0;
END

-- 2. StudioSections Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioSections')
BEGIN
    CREATE TABLE StudioSections (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        StudioPageId INT NOT NULL,
        Type NVARCHAR(50) NOT NULL,
        OrderIndex INT NOT NULL,
        Data NVARCHAR(MAX) NULL,
        AnimationSettingsJson NVARCHAR(MAX) NULL,
        CONSTRAINT FK_StudioSections_StudioPages FOREIGN KEY (StudioPageId) REFERENCES StudioPages(Id) ON DELETE CASCADE
    );
    PRINT 'Created StudioSections table.';
END
ELSE
BEGIN
    -- Rename columns if they exist under old names
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioSections') AND name = 'PageId')
        EXEC sp_rename 'StudioSections.PageId', 'StudioPageId', 'COLUMN';
    
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioSections') AND name = 'Title')
        EXEC sp_rename 'StudioSections.Title', 'Type', 'COLUMN';

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioSections') AND name = 'PageIndex')
        EXEC sp_rename 'StudioSections.PageIndex', 'OrderIndex', 'COLUMN';

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioSections') AND name = 'Content')
        EXEC sp_rename 'StudioSections.Content', 'Data', 'COLUMN';

    -- Ensure StudioPageId exists specifically (if it wasn't named PageId)
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioSections') AND name = 'StudioPageId')
        ALTER TABLE StudioSections ADD StudioPageId INT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioSections') AND name = 'AnimationSettingsJson')
        ALTER TABLE StudioSections ADD AnimationSettingsJson NVARCHAR(MAX) NULL;
END

-- 3. StudioDrafts Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioDrafts')
BEGIN
    CREATE TABLE StudioDrafts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        StudioPageId INT NOT NULL,
        ContentSnapshotJson NVARCHAR(MAX) NULL,
        CreatedBy NVARCHAR(MAX) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        Version INT NOT NULL DEFAULT 1,
        IsPublished BIT NOT NULL DEFAULT 0,
        PublishedAt DATETIME2 NULL,
        CONSTRAINT FK_StudioDrafts_StudioPages FOREIGN KEY (StudioPageId) REFERENCES StudioPages(Id) ON DELETE CASCADE
    );
    PRINT 'Created StudioDrafts table.';
END
ELSE
BEGIN
    -- Sync columns
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioDrafts') AND name = 'PageId')
        EXEC sp_rename 'StudioDrafts.PageId', 'StudioPageId', 'COLUMN';

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioDrafts') AND name = 'Payload')
        EXEC sp_rename 'StudioDrafts.Payload', 'ContentSnapshotJson', 'COLUMN';
    
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioDrafts') AND name = 'ContentJson')
        EXEC sp_rename 'StudioDrafts.ContentJson', 'ContentSnapshotJson', 'COLUMN';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioDrafts') AND name = 'StudioPageId')
        ALTER TABLE StudioDrafts ADD StudioPageId INT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioDrafts') AND name = 'ContentSnapshotJson')
        ALTER TABLE StudioDrafts ADD ContentSnapshotJson NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioDrafts') AND name = 'Version')
        ALTER TABLE StudioDrafts ADD Version INT NOT NULL DEFAULT 1;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioDrafts') AND name = 'IsPublished')
        ALTER TABLE StudioDrafts ADD IsPublished BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StudioDrafts') AND name = 'PublishedAt')
        ALTER TABLE StudioDrafts ADD PublishedAt DATETIME2 NULL;
END

PRINT 'Studio Database Reconciliation Complete.';
GO
