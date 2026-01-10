-- Check raw event data to see timestamp bytes
-- Run this on the HOST computer

USE [ClubMembership];
GO

SELECT TOP 5
    Id,
    TimestampUtc,
    RawIndex,
    RawData,
    -- Try to extract timestamp bytes if RawData is populated
    CASE 
        WHEN RawData IS NOT NULL AND LEN(RawData) >= 40 
        THEN SUBSTRING(RawData, 21, 14) -- Bytes 20-26 (7 bytes for timestamp)
        ELSE 'No RawData'
    END AS TimestampBytes
FROM ControllerEvents
ORDER BY Id DESC;
