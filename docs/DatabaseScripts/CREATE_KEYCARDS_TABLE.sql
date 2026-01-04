-- =============================================
-- GFC Key Cards System - Database Schema
-- Created: 2026-01-04
-- Description: Creates KeyCards table and related objects
-- =============================================

USE [ClubMembership];
GO

PRINT '========================================';
PRINT 'Creating KeyCards Table and Dependencies';
PRINT '========================================';
PRINT '';

-- Step 1: Create KeyCards table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KeyCards]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[KeyCards] (
        [KeyCardId] INT IDENTITY(1,1) NOT NULL,
        [MemberId] INT NOT NULL,
        [CardNumber] NVARCHAR(50) NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [IsControllerSynced] BIT NOT NULL DEFAULT 0,
        [LastControllerSyncDate] DATETIME2 NULL,
        [CardType] NVARCHAR(20) NULL, -- 'Card' or 'Fob'
        [Notes] NVARCHAR(500) NULL,
        [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_KeyCards] PRIMARY KEY CLUSTERED ([KeyCardId] ASC)
    );
    
    PRINT '✓ Created KeyCards table';
END
ELSE
BEGIN
    PRINT '✓ KeyCards table already exists';
    
    -- Ensure all columns exist (in case table was partially created)
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('KeyCards') AND name = 'IsControllerSynced')
    BEGIN
        ALTER TABLE [KeyCards] ADD [IsControllerSynced] BIT NOT NULL DEFAULT 0;
        PRINT '  → Added IsControllerSynced column';
    END
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('KeyCards') AND name = 'LastControllerSyncDate')
    BEGIN
        ALTER TABLE [KeyCards] ADD [LastControllerSyncDate] DATETIME2 NULL;
        PRINT '  → Added LastControllerSyncDate column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('KeyCards') AND name = 'CardType')
    BEGIN
        ALTER TABLE [KeyCards] ADD [CardType] NVARCHAR(20) NULL;
        PRINT '  → Added CardType column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('KeyCards') AND name = 'Notes')
    BEGIN
        ALTER TABLE [KeyCards] ADD [Notes] NVARCHAR(500) NULL;
        PRINT '  → Added Notes column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('KeyCards') AND name = 'CreatedDate')
    BEGIN
        ALTER TABLE [KeyCards] ADD [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
        PRINT '  → Added CreatedDate column';
    END
END
GO

-- Step 2: Create indexes for performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_KeyCards_MemberId' AND object_id = OBJECT_ID('KeyCards'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_KeyCards_MemberId] 
    ON [dbo].[KeyCards] ([MemberId] ASC);
    PRINT '✓ Created index IX_KeyCards_MemberId';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_KeyCards_CardNumber' AND object_id = OBJECT_ID('KeyCards'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_KeyCards_CardNumber] 
    ON [dbo].[KeyCards] ([CardNumber] ASC);
    PRINT '✓ Created index IX_KeyCards_CardNumber';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_KeyCards_IsActive' AND object_id = OBJECT_ID('KeyCards'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_KeyCards_IsActive] 
    ON [dbo].[KeyCards] ([IsActive] ASC);
    PRINT '✓ Created index IX_KeyCards_IsActive';
END
GO

-- Step 3: Create KeyCardHistory table for audit trail
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KeyCardHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[KeyCardHistory] (
        [HistoryId] INT IDENTITY(1,1) NOT NULL,
        [KeyCardId] INT NOT NULL,
        [MemberId] INT NOT NULL,
        [CardNumber] NVARCHAR(50) NOT NULL,
        [Action] NVARCHAR(50) NOT NULL, -- 'Assigned', 'Deactivated', 'Reactivated', 'Synced', etc.
        [DeviceType] NVARCHAR(20) NULL, -- 'Card' or 'Fob'
        [PerformedBy] NVARCHAR(100) NULL,
        [Timestamp] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [Notes] NVARCHAR(500) NULL,
        CONSTRAINT [PK_KeyCardHistory] PRIMARY KEY CLUSTERED ([HistoryId] ASC)
    );
    
    CREATE NONCLUSTERED INDEX [IX_KeyCardHistory_KeyCardId] 
    ON [dbo].[KeyCardHistory] ([KeyCardId] ASC);
    
    CREATE NONCLUSTERED INDEX [IX_KeyCardHistory_MemberId] 
    ON [dbo].[KeyCardHistory] ([MemberId] ASC);
    
    CREATE NONCLUSTERED INDEX [IX_KeyCardHistory_Timestamp] 
    ON [dbo].[KeyCardHistory] ([Timestamp] DESC);
    
    PRINT '✓ Created KeyCardHistory table';
END
ELSE
BEGIN
    PRINT '✓ KeyCardHistory table already exists';
    
    -- Ensure DeviceType column exists
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('KeyCardHistory') AND name = 'DeviceType')
    BEGIN
        ALTER TABLE [KeyCardHistory] ADD [DeviceType] NVARCHAR(20) NULL;
        PRINT '  → Added DeviceType column to KeyCardHistory';
    END
END
GO

-- Step 4: Ensure ControllerSyncQueue table exists (for background sync)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControllerSyncQueue]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ControllerSyncQueue] (
        [QueueId] INT IDENTITY(1,1) NOT NULL,
        [KeyCardId] INT NOT NULL,
        [CardNumber] NVARCHAR(50) NOT NULL,
        [Action] NVARCHAR(20) NOT NULL, -- 'ADD', 'DELETE', 'UPDATE'
        [QueuedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [AttemptCount] INT NOT NULL DEFAULT 0,
        [LastAttemptDate] DATETIME2 NULL,
        [LastError] NVARCHAR(500) NULL,
        [Status] NVARCHAR(20) NOT NULL DEFAULT 'PENDING', -- 'PENDING', 'PROCESSING', 'COMPLETED', 'FAILED'
        [CompletedDate] DATETIME2 NULL,
        CONSTRAINT [PK_ControllerSyncQueue] PRIMARY KEY CLUSTERED ([QueueId] ASC)
    );
    
    CREATE NONCLUSTERED INDEX [IX_SyncQueue_Status] 
    ON [dbo].[ControllerSyncQueue] ([Status] ASC);
    
    CREATE NONCLUSTERED INDEX [IX_SyncQueue_KeyCardId] 
    ON [dbo].[ControllerSyncQueue] ([KeyCardId] ASC);
    
    PRINT '✓ Created ControllerSyncQueue table';
END
ELSE
BEGIN
    PRINT '✓ ControllerSyncQueue table already exists';
END
GO

-- Step 5: Verify the schema
PRINT '';
PRINT '========================================';
PRINT 'Schema Verification';
PRINT '========================================';

-- Verify tables exist and show column counts
SELECT 
    t.name AS TableName,
    (SELECT COUNT(*) FROM sys.columns c WHERE c.object_id = t.object_id) AS ColumnCount,
    (SELECT COUNT(*) FROM sys.indexes i WHERE i.object_id = t.object_id AND i.is_primary_key = 0) AS IndexCount
FROM sys.tables t
WHERE t.name IN ('KeyCards', 'KeyCardHistory', 'ControllerSyncQueue')
ORDER BY t.name;
GO

PRINT '';
PRINT '========================================';
PRINT 'KeyCards Schema Creation Complete!';
PRINT '========================================';
PRINT '';
PRINT 'NEXT STEPS:';
PRINT '1. Verify tables were created above';
PRINT '2. Restart your application';
PRINT '3. Test KeyCard functionality';
PRINT '========================================';
GO
