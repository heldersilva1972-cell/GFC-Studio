-- Run this on the HOST computer (192.168.0.248)
-- Simple diagnostic to see what events exist

-- 1. How many events total?
SELECT COUNT(*) AS TotalEvents FROM ControllerEvents;

-- 2. What's the date range?
SELECT 
    MIN(TimestampUtc) AS OldestEvent,
    MAX(TimestampUtc) AS NewestEvent
FROM ControllerEvents;

-- 3. Show the 10 most recent events
SELECT TOP 10 
    Id,
    TimestampUtc,
    EventType,
    RawIndex,
    CardNumber
FROM ControllerEvents
ORDER BY TimestampUtc DESC;

-- 4. What's the sync status?
SELECT 
    c.Name AS ControllerName,
    c.SerialNumber,
    cli.LastRecordIndex AS LastSyncedIndex,
    (SELECT MAX(RawIndex) FROM ControllerEvents WHERE ControllerId = c.Id) AS MaxEventIndex
FROM Controllers c
LEFT JOIN ControllerLastIndexes cli ON c.Id = cli.ControllerId
WHERE c.IsEnabled = 1;
