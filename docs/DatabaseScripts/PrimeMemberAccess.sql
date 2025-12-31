-- SQL Script to Grant Default Door 1 Access to all active keycards
-- This unblocks the Sync Queue "Retry Now" functionality for existing items.

USE ClubMembership;
GO

-- 1. Identify the 'Main' door for each controller (DoorIndex = 1)
-- 2. For every active KeyCard that doesn't have any permissions yet...
-- 3. Insert a record into MemberDoorAccess for Door 1.

INSERT INTO dbo.MemberDoorAccess (MemberId, DoorId, CardNumber, IsEnabled)
SELECT 
    k.MemberID, 
    d.Id as DoorId, 
    k.CardNumber, 
    1 as IsEnabled
FROM dbo.KeyCards k
CROSS JOIN (SELECT Id FROM dbo.Doors WHERE DoorIndex = 1) d
LEFT JOIN dbo.MemberDoorAccess mda ON k.CardNumber = mda.CardNumber AND d.Id = mda.DoorId
WHERE k.IsActive = 1 
  AND mda.Id IS NULL; -- Only if they don't have it already

PRINT 'Granted default Door 1 access to all active keycards missing permissions.';
GO
