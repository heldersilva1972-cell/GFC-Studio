-- Clean verification of Door configuration
SELECT 
    Id, 
    Name, 
    DoorIndex, 
    ControllerId 
FROM Doors;

-- Also check for any orphan events that don't have a door match
SELECT 
    COUNT(*) as EventsWithNoDoor,
    DoorOrReader as HardwareReportedIndex
FROM ControllerEvents
WHERE DoorId IS NULL
GROUP BY DoorOrReader;
