-- =============================================
-- Fix: Force Deactivate Card #123
-- Description: Deactivates card #123 regardless of member ID
-- =============================================

USE ClubMembership;
GO

-- Check update count
DECLARE @Rows INT;

UPDATE dbo.KeyCards
SET IsActive = 0
WHERE CardNumber = '123';

SET @Rows = @@ROWCOUNT;
PRINT 'Deactivated ' + CAST(@Rows AS NVARCHAR(10)) + ' card(s) with number 123.';

-- Verify what remains active for Member 12
SELECT KeyCardId, MemberID, CardNumber, IsActive 
FROM dbo.KeyCards 
WHERE MemberID = 12 AND IsActive = 1;
GO
