-- Create ControllerEvents table and related tables
-- Run this on the HOST database

-- 1. Create ControllerEvents table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ControllerEvents')
BEGIN
    CREATE TABLE [dbo].[ControllerEvents] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ControllerId] INT NOT NULL,
        [DoorId] INT NULL,
        [TimestampUtc] DATETIME2 NOT NULL,
        [CardNumber] BIGINT NULL,
        [EventType] INT NOT NULL,
        [ReasonCode] INT NULL,
        [IsByCard] BIT NOT NULL DEFAULT 0,
        [IsByButton] BIT NOT NULL DEFAULT 0,
        [RawIndex] INT NOT NULL,
        [RawData] NVARCHAR(4000) NULL,
        [IsSimulated] BIT NOT NULL DEFAULT 0,
        [DoorOrReader] INT NOT NULL DEFAULT 0,
        [CreatedUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT FK_ControllerEvents_Controller FOREIGN KEY ([ControllerId]) 
            REFERENCES [Controllers]([Id]) ON DELETE NO ACTION,
        CONSTRAINT FK_ControllerEvents_Door FOREIGN KEY ([DoorId]) 
            REFERENCES [Doors]([Id]) ON DELETE NO ACTION
    );
    
    -- Add performance indices
    CREATE INDEX IX_ControllerEvents_Sync ON ControllerEvents (ControllerId, RawIndex);
    CREATE INDEX IX_ControllerEvents_Recent ON ControllerEvents (ControllerId, TimestampUtc DESC, RawIndex DESC);
    
    PRINT 'Created ControllerEvents table';
END
ELSE
BEGIN
    PRINT 'ControllerEvents table already exists';
END
GO

-- 2. Create ControllerLastIndexes tracking table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ControllerLastIndexes')
BEGIN
    CREATE TABLE [dbo].[ControllerLastIndexes] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ControllerId] INT NOT NULL UNIQUE,
        [LastRecordIndex] BIGINT NOT NULL DEFAULT 0,
        [UpdatedUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT FK_ControllerLastIndexes_Controller FOREIGN KEY ([ControllerId]) 
            REFERENCES [Controllers]([Id]) ON DELETE CASCADE
    );
    
    PRINT 'Created ControllerLastIndexes table';
END
ELSE
BEGIN
    PRINT 'ControllerLastIndexes table already exists';
END
GO

PRINT 'Event tracking schema created successfully';
