-- Performance Optimization for Key Card Management Page
-- Add index to speed up card loading

-- Check if index already exists and drop it
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_KeyCards_MemberId_IsActive')
    DROP INDEX IX_KeyCards_MemberId_IsActive ON KeyCards;

-- Index on KeyCards for faster lookups
-- Index on KeyCards for faster lookups
CREATE NONCLUSTERED INDEX IX_KeyCards_MemberId_IsActive 
ON KeyCards(MemberId, IsActive);

-- CRITICAL: Indexes for ControllerEvents to speed up Key Card Management loading
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ControllerEvents_CardNumber_Timestamp')
    CREATE NONCLUSTERED INDEX IX_ControllerEvents_CardNumber_Timestamp 
    ON ControllerEvents(CardNumber, TimestampUtc DESC);

-- Index for synchronization logic (SyncFromControllerAsync)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ControllerEvents_ControllerId_RawIndex')
    CREATE NONCLUSTERED INDEX IX_ControllerEvents_ControllerId_RawIndex 
    ON ControllerEvents(ControllerId, RawIndex DESC);

PRINT 'Performance indexes created successfully';
