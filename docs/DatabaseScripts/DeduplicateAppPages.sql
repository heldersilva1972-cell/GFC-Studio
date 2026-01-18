-- DEDUPLICATION AND REPAIR SCRIPT FOR APP PAGES
-- This script cleans up duplicate records and ensures the Finance section is correct.

-- 1. Remove exact duplicates based on PageRoute (keeping the first one)
WITH CTE AS (
    SELECT 
        PageId, 
        PageRoute, 
        ROW_NUMBER() OVER(PARTITION BY PageRoute ORDER BY PageId) as rn
    FROM AppPages
)
DELETE FROM AppPages 
WHERE PageId IN (SELECT PageId FROM CTE WHERE rn > 1);

-- 2. Force all Categories to ALL CAPS to prevent duplicate UI grouping
UPDATE AppPages SET Category = UPPER(LTRIM(RTRIM(Category)));

-- 3. Fix specific UI groups that have spaces or special naming
UPDATE AppPages SET Category = 'HALL RENTALS' WHERE Category = 'HALLRENTALS' OR Category = 'HALL RENTAL';
UPDATE AppPages SET Category = 'CAMERA SYSTEM' WHERE Category = 'CAMERASYSTEM' OR Category = 'CAMERAS';
UPDATE AppPages SET Category = 'GFC STUDIO' WHERE Category = 'GFCSTUDIO' OR Category = 'STUDIO';

-- 4. Specifically fix the Finance routes to ensure they are in the 'FINANCE' group
UPDATE AppPages SET Category = 'FINANCE' 
WHERE PageRoute LIKE '%finance%' 
   OR PageRoute IN ('/reimbursements', '/manage-reimbursements', '/reports', '/lottery-sales', '/bar-sales', '/bartender-shift', '/lottery');

-- 5. Explicitly check for Financial Insights
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/finance/insights')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Financial Insights', '/finance/insights', 'In-depth financial performance analysis', 'FINANCE', 0, 1, 36);
END
ELSE
BEGIN
    UPDATE AppPages SET Category = 'FINANCE', PageName = 'Financial Insights', IsActive = 1 
    WHERE PageRoute = '/finance/insights';
END

-- 6. Cleanup any 'UNSPECIFIED' or NULLs
UPDATE AppPages SET Category = 'FINANCE' WHERE PageRoute = '/finance/insights' AND (Category IS NULL OR Category = 'UNSPECIFIED');

GO
