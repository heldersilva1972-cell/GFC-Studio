USE ClubMembership;
GO

PRINT 'Before cleanup:';
SELECT 'ControllerLastIndexes' as TableName, COUNT(*) as RowCount FROM ControllerLastIndexes;
SELECT 'ControllerEvents' as TableName, COUNT(*) as RowCount FROM ControllerEvents;
GO

-- Delete all sync state
DELETE FROM ControllerLastIndexes;
DELETE FROM ControllerEvents;
GO

PRINT 'After cleanup:';
SELECT 'ControllerLastIndexes' as TableName, COUNT(*) as RowCount FROM ControllerLastIndexes;
SELECT 'ControllerEvents' as TableName, COUNT(*) as RowCount FROM ControllerEvents;
GO

PRINT 'Cleanup complete. Restart the application to trigger a fresh sync.';
GO
