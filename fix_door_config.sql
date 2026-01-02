-- Fix Door Configuration
-- Only Door 2 is physically connected to the controller

-- Show current state
SELECT 'CURRENT CONFIGURATION:' as Status;
SELECT Id, Name, DoorIndex, IsEnabled, ControllerId FROM Doors ORDER BY DoorIndex;

-- Option 1: Disable Door 1 (keep it in database but mark as not connected)
UPDATE Doors 
SET IsEnabled = 0,
    Name = Name + ' (Not Connected)'
WHERE DoorIndex = 1 AND IsEnabled = 1;

-- Option 2: If you want to delete Door 1 entirely (uncomment if needed)
-- DELETE FROM Doors WHERE DoorIndex = 1;

-- Ensure Door 2 is enabled and properly named
UPDATE Doors 
SET IsEnabled = 1,
    Name = REPLACE(Name, ' (Not Connected)', '')
WHERE DoorIndex = 2;

-- Show result
SELECT 'AFTER FIX:' as Status;
SELECT Id, Name, DoorIndex, IsEnabled, ControllerId FROM Doors ORDER BY DoorIndex;

-- Clean up any orphaned access records for Door 1
DELETE FROM MemberDoorAccesses WHERE DoorId IN (SELECT Id FROM Doors WHERE DoorIndex = 1);

PRINT 'Fix complete. Only Door 2 is now active (the physically connected door).';
