-- Check Door Configuration
SELECT 
    d.Id,
    d.Name,
    d.DoorIndex,
    d.ControllerId,
    d.IsEnabled,
    c.Name as ControllerName
FROM Doors d
LEFT JOIN ControllerDevices c ON d.ControllerId = c.Id
ORDER BY c.Id, d.DoorIndex;

-- Check if Door 1 has the correct DoorIndex
SELECT 
    Name,
    DoorIndex,
    CASE 
        WHEN DoorIndex = 1 THEN 'CORRECT'
        ELSE 'WRONG - Should be 1'
    END as Status
FROM Doors
WHERE Name LIKE '%Main%' OR Name LIKE '%Door 1%' OR Name LIKE '%Front%';
