-- Add unique constraint to prevent duplicate events with the same RawIndex
-- This ensures each hardware event is only stored once in the database

-- First, delete any existing duplicates
WITH CTE AS (
    SELECT 
        Id,
        ROW_NUMBER() OVER (PARTITION BY ControllerId, RawIndex ORDER BY Id) AS RowNum
    FROM ControllerEvents
)
DELETE FROM CTE WHERE RowNum > 1;

-- Now add the unique constraint
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_ControllerEvents_ControllerId_RawIndex' 
    AND object_id = OBJECT_ID('ControllerEvents')
)
BEGIN
    CREATE UNIQUE INDEX IX_ControllerEvents_ControllerId_RawIndex 
    ON ControllerEvents(ControllerId, RawIndex);
END
