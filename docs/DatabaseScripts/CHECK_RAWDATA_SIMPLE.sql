-- Simple check for RawData
SELECT TOP 5
    Id,
    TimestampUtc,
    RawIndex,
    CASE 
        WHEN RawData IS NULL THEN 'NULL'
        WHEN RawData = '' THEN 'EMPTY'
        ELSE LEFT(RawData, 50) + '...'
    END AS RawDataPreview
FROM ControllerEvents
ORDER BY Id DESC;
