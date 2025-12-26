USE [ClubMembership]
GO

PRINT 'Starting PublicReviews Table Check...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PublicReviews')
BEGIN
    PRINT 'Creating PublicReviews table...';
    CREATE TABLE [dbo].[PublicReviews] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [DisplayName] NVARCHAR(100) NULL,
        [EventType] NVARCHAR(100) NULL,
        [Content] NVARCHAR(2000) NULL,
        [IsApproved] BIT NOT NULL DEFAULT 0,
        [IsFeatured] BIT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [Name] NVARCHAR(MAX) NULL,
        [Rating] INT NOT NULL DEFAULT 0,
        [Comment] NVARCHAR(MAX) NULL
    );
END
ELSE
BEGIN
    PRINT 'PublicReviews table exists. Checking columns...';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PublicReviews]') AND name = 'DisplayName')
        ALTER TABLE [dbo].[PublicReviews] ADD [DisplayName] NVARCHAR(100) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PublicReviews]') AND name = 'EventType')
        ALTER TABLE [dbo].[PublicReviews] ADD [EventType] NVARCHAR(100) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PublicReviews]') AND name = 'Content')
        ALTER TABLE [dbo].[PublicReviews] ADD [Content] NVARCHAR(2000) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PublicReviews]') AND name = 'IsFeatured')
        ALTER TABLE [dbo].[PublicReviews] ADD [IsFeatured] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PublicReviews]') AND name = 'Name')
        ALTER TABLE [dbo].[PublicReviews] ADD [Name] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PublicReviews]') AND name = 'Rating')
        ALTER TABLE [dbo].[PublicReviews] ADD [Rating] INT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PublicReviews]') AND name = 'Comment')
        ALTER TABLE [dbo].[PublicReviews] ADD [Comment] NVARCHAR(MAX) NULL;
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PublicReviews]') AND name = 'CreatedAt')
        ALTER TABLE [dbo].[PublicReviews] ADD [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
END
GO

PRINT 'PublicReviews Table Check Complete.';
GO
