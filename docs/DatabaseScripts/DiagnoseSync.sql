USE ClubMembership;

PRINT '=== Current Database State ===';
PRINT '';

PRINT 'Controllers:';
SELECT Id, SerialNumber, Name, IsEnabled FROM Controllers;
PRINT '';

PRINT 'ControllerLastIndexes:';
SELECT * FROM ControllerLastIndexes;
PRINT '';

PRINT 'ControllerEvents Count:';
SELECT COUNT(*) as TotalEvents FROM ControllerEvents;
PRINT '';

PRINT 'Latest ControllerEvents:';
SELECT TOP 5 Id, ControllerId, RawIndex, CardNumber, EventType, TimestampUtc, CreatedUtc 
FROM ControllerEvents 
ORDER BY Id DESC;
PRINT '';

PRINT '=== Expected Values ===';
PRINT 'Controller Index from trace: 150939168 (0x09012620)';
PRINT 'If LastRecordIndex = 150939168, Gap = 0 (no sync will run)';
PRINT 'If LastRecordIndex = 0, will sync last 1000 events (150938168 to 150939168)';
