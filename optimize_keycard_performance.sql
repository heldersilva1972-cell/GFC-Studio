-- Performance Optimization for Key Card Management Page
-- Add index to speed up card loading

-- Check if index already exists and drop it
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_KeyCards_MemberId_IsActive')
    DROP INDEX IX_KeyCards_MemberId_IsActive ON KeyCards;

-- Index on KeyCards for faster lookups
CREATE NONCLUSTERED INDEX IX_KeyCards_MemberId_IsActive 
ON KeyCards(MemberId, IsActive);

PRINT 'Performance index created successfully';
