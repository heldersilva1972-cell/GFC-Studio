USE ClubMembership;
GO

BEGIN TRANSACTION;

PRINT 'Cleaning up user-facing Reimbursement pages...';

-- 1. Identify PageIds for the pages we want to remove
DECLARE @PageIds TABLE (PageId INT);
INSERT INTO @PageIds (PageId)
SELECT PageId FROM AppPages 
WHERE PageRoute IN ('/reimbursements', '/reimbursements/new', '/reimbursements/reports-obsolete', '/reimbursements/settings');

-- 2. Remove permissions associated with these pages
DELETE FROM UserPagePermissions 
WHERE PageId IN (SELECT PageId FROM @PageIds);

-- 3. Remove default permissions associated with these pages
IF OBJECT_ID(N'[dbo].[DefaultPermissions]', N'U') IS NOT NULL
BEGIN
    DELETE FROM DefaultPermissions 
    WHERE PageId IN (SELECT PageId FROM @PageIds);
END

-- 4. Remove the pages themselves
DELETE FROM AppPages 
WHERE PageId IN (SELECT PageId FROM @PageIds);

COMMIT;

-- Verification
PRINT 'Verification: Remaining Reimbursement Pages';
SELECT PageId, PageName, PageRoute, IsActive 
FROM AppPages 
WHERE PageRoute LIKE '/reimbursements%';
GO
