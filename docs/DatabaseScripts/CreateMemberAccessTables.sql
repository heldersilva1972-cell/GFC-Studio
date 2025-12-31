USE [ClubMembership];
GO

PRINT 'Ensuring Member Door Access tables exist...';

-- 1. Create MemberDoorAccess table if missing
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MemberDoorAccess')
BEGIN
    CREATE TABLE [dbo].[MemberDoorAccess] (
        [Id] INT IDENTITY(1, 1) NOT NULL,
        [MemberId] INT NOT NULL,
        [DoorId] INT NOT NULL,
        [CardNumber] NVARCHAR(50) NOT NULL,
        [TimeProfileId] INT NULL,
        [IsEnabled] BIT NOT NULL DEFAULT 1,
        [LastSyncedAt] DATETIME2 NULL,
        [LastSyncResult] NVARCHAR(500) NULL,
        CONSTRAINT [PK_MemberDoorAccess] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_MemberDoorAccess_Doors_DoorId] FOREIGN KEY ([DoorId]) REFERENCES [dbo].[Doors] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_MemberDoorAccess_MemberId_DoorId_CardNumber] ON [dbo].[MemberDoorAccess] ([MemberId], [DoorId], [CardNumber]);
    CREATE INDEX [IX_MemberDoorAccess_DoorId] ON [dbo].[MemberDoorAccess] ([DoorId]);
    
    PRINT '✓ Created MemberDoorAccess table.';
END
ELSE
BEGIN
    PRINT '✓ MemberDoorAccess table already exists.';
    
    -- Ensure all columns exist (in case it was partially created)
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('MemberDoorAccess') AND name = 'LastSyncedAt')
        ALTER TABLE [MemberDoorAccess] ADD [LastSyncedAt] DATETIME2 NULL;
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('MemberDoorAccess') AND name = 'LastSyncResult')
        ALTER TABLE [MemberDoorAccess] ADD [LastSyncResult] NVARCHAR(500) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('MemberDoorAccess') AND name = 'CardNumber')
        ALTER TABLE [MemberDoorAccess] ADD [CardNumber] NVARCHAR(50) NOT NULL DEFAULT '';
END
GO

-- 2. Ensure ControllerSyncQueue table exists (if not already handled by other scripts)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ControllerSyncQueue')
BEGIN
    CREATE TABLE [dbo].[ControllerSyncQueue] (
        [QueueId] INT IDENTITY(1, 1) NOT NULL,
        [KeyCardId] INT NOT NULL,
        [CardNumber] NVARCHAR(50) NOT NULL,
        [Action] NVARCHAR(20) NOT NULL,
        [QueuedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [AttemptCount] INT NOT NULL DEFAULT 0,
        [LastAttemptDate] DATETIME NULL,
        [LastError] NVARCHAR(500) NULL,
        [Status] NVARCHAR(20) NOT NULL DEFAULT 'PENDING',
        [CompletedDate] DATETIME NULL,
        CONSTRAINT [PK_ControllerSyncQueue] PRIMARY KEY CLUSTERED ([QueueId] ASC)
    );
    
    CREATE INDEX [IX_SyncQueue_Status] ON [dbo].[ControllerSyncQueue] ([Status]);
    PRINT '✓ Created ControllerSyncQueue table.';
END
GO

-- 3. Ensure DoorConfigs has NEW hardware columns
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'DoorConfigs')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('DoorConfigs') AND name = 'ControlMode')
        ALTER TABLE [DoorConfigs] ADD [ControlMode] TINYINT NOT NULL DEFAULT 3;
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('DoorConfigs') AND name = 'SensorType')
        ALTER TABLE [DoorConfigs] ADD [SensorType] TINYINT NOT NULL DEFAULT 1;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('DoorConfigs') AND name = 'Interlock')
        ALTER TABLE [DoorConfigs] ADD [Interlock] TINYINT NOT NULL DEFAULT 0;
        
    PRINT '✓ Verified DoorConfigs hardware columns.';
END
GO

PRINT 'Member Door Access tables verification complete.';
