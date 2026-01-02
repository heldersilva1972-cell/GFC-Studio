-- Fix Door 1 DoorIndex
-- The N3000 controller expects DoorIndex to be 1-4, not 0-3

-- First, let's see what we have
SELECT 'BEFORE FIX:' as Status;
SELECT Id, Name, DoorIndex, ControllerId FROM Doors ORDER BY ControllerId, DoorIndex;

-- Fix any doors with DoorIndex = 0 to be DoorIndex = 1
UPDATE Doors 
SET DoorIndex = 1 
WHERE DoorIndex = 0 OR DoorIndex IS NULL;

-- Show the result
SELECT 'AFTER FIX:' as Status;
SELECT Id, Name, DoorIndex, ControllerId FROM Doors ORDER BY ControllerId, DoorIndex;
