-- Ensure ControllerLastIndexes table exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControllerLastIndexes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ControllerLastIndexes](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ControllerId] [int] NOT NULL,
        [LastRecordIndex] [bigint] NOT NULL,
        CONSTRAINT [PK_ControllerLastIndexes] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT 'Created ControllerLastIndexes table';
END
ELSE
BEGIN
    PRINT 'ControllerLastIndexes table already exists';
END

-- Ensure ControllerEvents indexes exist for performance (Key Card Management loading)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ControllerEvents_CardNumber_Timestamp' AND object_id = OBJECT_ID('ControllerEvents'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ControllerEvents_CardNumber_Timestamp 
    ON ControllerEvents(CardNumber, TimestampUtc DESC);
    PRINT 'Created IX_ControllerEvents_CardNumber_Timestamp index';
END
ELSE
BEGIN
    PRINT 'IX_ControllerEvents_CardNumber_Timestamp index already exists';
END

-- Ensure KeyCards index exists
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_KeyCards_MemberId_IsActive' AND object_id = OBJECT_ID('KeyCards'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_KeyCards_MemberId_IsActive 
    ON KeyCards(MemberId, IsActive);
    PRINT 'Created IX_KeyCards_MemberId_IsActive index';
END
ELSE
BEGIN
    PRINT 'IX_KeyCards_MemberId_IsActive index already exists';
END

-- Ensure ControllerId + RawIndex for fast sync lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ControllerEvents_ControllerId_RawIndex' AND object_id = OBJECT_ID('ControllerEvents'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ControllerEvents_ControllerId_RawIndex 
    ON ControllerEvents(ControllerId, RawIndex DESC);
    PRINT 'Created IX_ControllerEvents_ControllerId_RawIndex index';
END
ELSE
BEGIN
    PRINT 'IX_ControllerEvents_ControllerId_RawIndex index already exists';
END
