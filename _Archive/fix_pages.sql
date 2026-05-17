USE ClubMembership;
GO

BEGIN TRANSACTION;

-- 1. Clean up Lottery Sales completely
PRINT 'Cleaning up Lottery Sales...';
DELETE FROM UserPagePermissions 
WHERE PageId IN (SELECT PageId FROM AppPages WHERE PageRoute = '/lottery' OR PageName LIKE '%Lottery Sales%');

DELETE FROM AppPages 
WHERE PageRoute = '/lottery' OR PageName LIKE '%Lottery Sales%';

-- 2. Rename Bar Sales Entry to Bar/Lottery Sales
PRINT 'Renaming Bar Sales Entry...';
UPDATE AppPages 
SET PageName = 'Bar/Lottery Sales', 
    Description = 'Bar and Lottery sales entry and reporting',
    IsActive = 1
WHERE PageRoute = '/admin/bar-sales' OR PageName = 'Bar Sales Entry';

COMMIT;

-- 3. Verification
PRINT 'Verification Results:';
SELECT PageId, PageName, PageRoute, IsActive FROM AppPages 
WHERE PageRoute IN ('/lottery', '/admin/bar-sales');
GO
