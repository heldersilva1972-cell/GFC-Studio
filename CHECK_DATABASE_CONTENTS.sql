-- Check what's in the database after initialization
-- Run this on the HOST computer (192.168.0.248)

USE [ClubMembership];
GO

PRINT '========================================';
PRINT 'DATABASE CONTENTS REPORT';
PRINT '========================================';
PRINT '';

-- 1. Controllers
PRINT '1. CONTROLLERS:';
SELECT 
    Id,
    Name,
    SerialNumber,
    IpAddress,
    IsEnabled
FROM Controllers;
PRINT '';

-- 2. Doors
PRINT '2. DOORS:';
SELECT 
    d.Id,
    d.Name,
    d.DoorIndex,
    c.Name AS ControllerName,
    d.IsEnabled
FROM Doors d
LEFT JOIN Controllers c ON d.ControllerId = c.Id;
PRINT '';

-- 3. Controller Events (total count)
PRINT '3. CONTROLLER EVENTS:';
SELECT COUNT(*) AS TotalEvents FROM ControllerEvents;
PRINT '';

-- 4. Latest 10 events
PRINT '4. LATEST 10 EVENTS:';
SELECT TOP 10
    e.Id,
    c.Name AS Controller,
    d.Name AS Door,
    e.TimestampUtc,
    e.EventType,
    e.CardNumber,
    e.RawIndex
FROM ControllerEvents e
LEFT JOIN Controllers c ON e.ControllerId = c.Id
LEFT JOIN Doors d ON e.DoorId = d.Id
ORDER BY e.TimestampUtc DESC, e.RawIndex DESC;
PRINT '';

-- 5. Event date range
PRINT '5. EVENT DATE RANGE:';
SELECT 
    MIN(TimestampUtc) AS OldestEvent,
    MAX(TimestampUtc) AS NewestEvent,
    COUNT(*) AS TotalEvents
FROM ControllerEvents;
PRINT '';

-- 6. Sync status
PRINT '6. SYNC STATUS:';
SELECT 
    c.Name AS ControllerName,
    c.SerialNumber,
    ISNULL(cli.LastRecordIndex, 0) AS LastSyncedIndex,
    ISNULL((SELECT MAX(RawIndex) FROM ControllerEvents WHERE ControllerId = c.Id), 0) AS MaxEventIndex,
    ISNULL((SELECT COUNT(*) FROM ControllerEvents WHERE ControllerId = c.Id), 0) AS EventsInDB
FROM Controllers c
LEFT JOIN ControllerLastIndexes cli ON c.Id = cli.ControllerId
WHERE c.IsEnabled = 1;
PRINT '';

PRINT '========================================';
PRINT 'END OF REPORT';
PRINT '========================================';
