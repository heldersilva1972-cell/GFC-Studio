-- =============================================
-- Fix: Deactivate Duplicate Card for Mike Aptt
-- Description: Deactivates the old test card (#123) for Member #12 so only the new one remains active.
-- =============================================

USE ClubMembership;
GO

-- Deactivate the old card #123
UPDATE dbo.KeyCards
SET IsActive = 0
WHERE MemberID = 12 
  AND CardNumber = '123';

PRINT 'Successfully deactivated card #123 for Mike Aptt. It will no longer appear in the active list.';
GO
