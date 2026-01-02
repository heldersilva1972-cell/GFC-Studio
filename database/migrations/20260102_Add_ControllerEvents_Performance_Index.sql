-- Performance Optimization: Add index to ControllerEvents table
-- This index dramatically improves the performance of the "Last Used" query in CardAssignmentsTab
-- Expected improvement: 15 seconds -> ~500ms

USE [ClubMembership]
GO

-- Check if index already exists
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ControllerEvents_CardNumber_TimestampUtc' AND object_id = OBJECT_ID('dbo.ControllerEvents'))
BEGIN
    PRINT 'Creating index IX_ControllerEvents_CardNumber_TimestampUtc...'
    
    CREATE NONCLUSTERED INDEX [IX_ControllerEvents_CardNumber_TimestampUtc]
    ON [dbo].[ControllerEvents] ([CardNumber], [TimestampUtc] DESC)
    INCLUDE ([EventType])
    WITH (ONLINE = OFF, FILLFACTOR = 90)
    
    PRINT 'Index created successfully!'
END
ELSE
BEGIN
    PRINT 'Index IX_ControllerEvents_CardNumber_TimestampUtc already exists.'
END
GO

-- Verify the index was created
SELECT 
    i.name AS IndexName,
    i.type_desc AS IndexType,
    i.is_unique AS IsUnique,
    COL_NAME(ic.object_id, ic.column_id) AS ColumnName,
    ic.key_ordinal AS KeyOrdinal,
    ic.is_included_column AS IsIncludedColumn
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
WHERE i.object_id = OBJECT_ID('dbo.ControllerEvents')
    AND i.name = 'IX_ControllerEvents_CardNumber_TimestampUtc'
ORDER BY ic.key_ordinal, ic.is_included_column
GO

PRINT 'Performance optimization complete!'
GO
