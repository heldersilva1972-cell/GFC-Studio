-- SQL Script to manually create GFC Studio tables if migrations were skipped
-- Run this in your SQL Server Management Studio (SSMS) against the GFC database

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioPages')
BEGIN
    CREATE TABLE StudioPages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(MAX) NOT NULL,
        Content NVARCHAR(MAX) NULL,
        IsPublished BIT NOT NULL DEFAULT 0
    );
    PRINT 'Table StudioPages created.';
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
    PRINT 'Table StudioSections created.';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioDrafts')
BEGIN
    CREATE TABLE StudioDrafts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        PageId INT NOT NULL,
        ContentSnapshotJson NVARCHAR(MAX) NULL,
        CreatedBy NVARCHAR(MAX) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_StudioDrafts_StudioPages FOREIGN KEY (PageId) 
            REFERENCES StudioPages(Id) ON DELETE CASCADE
    );
    PRINT 'Table StudioDrafts created.';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioTemplates')
BEGIN
    CREATE TABLE StudioTemplates (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Category NVARCHAR(50) NOT NULL,
        ContentJson NVARCHAR(MAX) NOT NULL,
        ThumbnailUrl NVARCHAR(MAX) NULL
    );
    PRINT 'Table StudioTemplates created.';
END
