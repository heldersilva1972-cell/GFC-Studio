-- Check what doors are configured and which ones have access for card 5798887
SELECT 
    'DOOR CONFIGURATION:' as Info;
    
SELECT 
    d.Id,
    d.Name,
    d.DoorIndex,
    d.IsEnabled,
    d.ControllerId
FROM Doors d
ORDER BY d.DoorIndex;

SELECT 
    'MEMBER DOOR ACCESS FOR CARD 5798887:' as Info;

SELECT 
    mda.Id,
    d.Name as DoorName,
    d.DoorIndex,
    mda.CardNumber,
    mda.IsEnabled as AccessEnabled,
    mda.LastSyncedAt,
    mda.LastSyncResult
FROM MemberDoorAccesses mda
JOIN Doors d ON mda.DoorId = d.Id
WHERE mda.CardNumber = '5798887'
ORDER BY d.DoorIndex;
