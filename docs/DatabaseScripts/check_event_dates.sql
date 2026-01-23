-- Check what events exist in the database
PRINT '=== CONTROLLER EVENTS DIAGNOSTIC ==='
PRINT ''

-- 1. Total event count
PRINT '1. Total Events in Database:'
SELECT COUNT(*) AS TotalEvents FROM ControllerEvents
PRINT ''

-- 2. Latest 10 events by timestamp
PRINT '2. Latest 10 Events (by Timestamp):'
SELECT TOP 10 
    Id,
    ControllerId,
    TimestampUtc,
    EventType,
    RawIndex,
    DoorOrReader,
    CardNumber,
    CreatedUtc
FROM ControllerEvents
ORDER BY TimestampUtc DESC, RawIndex DESC
PRINT ''

-- 3. Date range of events
PRINT '3. Event Date Range:'
SELECT 
    MIN(TimestampUtc) AS OldestEvent,
    MAX(TimestampUtc) AS NewestEvent,
    DATEDIFF(day, MIN(TimestampUtc), MAX(TimestampUtc)) AS DaysCovered
FROM ControllerEvents
PRINT ''

-- 4. Events per day (last 14 days)
PRINT '4. Events Per Day (Last 14 Days):'
SELECT 
    CAST(TimestampUtc AS DATE) AS EventDate,
    COUNT(*) AS EventCount
FROM ControllerEvents
WHERE TimestampUtc >= DATEADD(day, -14, GETUTCDATE())
GROUP BY CAST(TimestampUtc AS DATE)
ORDER BY EventDate DESC
PRINT ''

-- 5. Controller sync status
PRINT '5. Controller Sync Status:'
SELECT 
    c.Id AS ControllerId,
    c.Name AS ControllerName,
    c.SerialNumber,
    cli.LastRecordIndex,
    (SELECT COUNT(*) FROM ControllerEvents WHERE ControllerId = c.Id) AS EventsInDB,
    (SELECT MAX(RawIndex) FROM ControllerEvents WHERE ControllerId = c.Id) AS MaxRawIndex
FROM Controllers c
LEFT JOIN ControllerLastIndexes cli ON c.Id = cli.ControllerId
WHERE c.IsEnabled = 1
PRINT ''

PRINT '=== END DIAGNOSTIC ==='
