-- Check door mapping
SELECT TOP 5
    e.Id,
    e.DoorOrReader AS RawDoorNumber,
    e.DoorId,
    d.Name AS DoorName,
    d.DoorIndex AS DoorIndexInDB
FROM ControllerEvents e
LEFT JOIN Doors d ON e.DoorId = d.Id
ORDER BY e.Id DESC;
