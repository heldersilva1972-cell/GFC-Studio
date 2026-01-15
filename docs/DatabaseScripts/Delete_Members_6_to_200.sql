-- =============================================
-- Delete Members with IDs 6-200
-- Created: 2026-01-15
-- Purpose: Remove duplicate members created during import testing
-- =============================================

-- IMPORTANT: Review this script before running!
-- This will permanently delete members and all related data

BEGIN TRANSACTION;

DECLARE @DeletedCount INT = 0;

-- Delete related records first (to avoid foreign key violations)

-- 1. Delete Dues Payments
DELETE FROM DuesPayments 
WHERE MemberID BETWEEN 6 AND 200;

-- 2. Delete Key Card Assignments
DELETE FROM MemberKeycardAssignments 
WHERE MemberId BETWEEN 6 AND 200;

-- 3. Delete Physical Key Assignments
DELETE FROM PhysicalKeyAssignments 
WHERE MemberID BETWEEN 6 AND 200;

-- 4. Delete Board Member Records
DELETE FROM BoardMembers 
WHERE MemberID BETWEEN 6 AND 200;

-- 5. Delete from Non-Portuguese Queue (if exists)
IF OBJECT_ID('NonPortugueseQueue', 'U') IS NOT NULL
BEGIN
    DELETE FROM NonPortugueseQueue 
    WHERE MemberID BETWEEN 6 AND 200;
END

-- 6. Delete from Member Change History (if exists)
IF OBJECT_ID('MemberChangeHistory', 'U') IS NOT NULL
BEGIN
    DELETE FROM MemberChangeHistory 
    WHERE MemberID BETWEEN 6 AND 200;
END

-- 7. Finally, delete the members themselves
DELETE FROM Members 
WHERE MemberID BETWEEN 6 AND 200;

SET @DeletedCount = @@ROWCOUNT;

-- Show summary
SELECT @DeletedCount AS 'Members Deleted';

-- Review the transaction before committing
-- If everything looks correct, run: COMMIT TRANSACTION
-- If you want to undo, run: ROLLBACK TRANSACTION

PRINT 'Transaction is open. Review the results above.';
PRINT 'To commit: Run COMMIT TRANSACTION';
PRINT 'To undo: Run ROLLBACK TRANSACTION';

-- Uncomment the next line to auto-commit (USE WITH CAUTION!)
-- COMMIT TRANSACTION;
