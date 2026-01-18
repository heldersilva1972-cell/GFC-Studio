-- MASTER CLEANUP AND REPAIR FOR APP PAGES
-- This script fixes duplicates and standardizes the permission system.

SET NOCOUNT ON;

-- 1. Standardize text fields (Trim and Upper) to ensure matching logic works
UPDATE AppPages SET 
    PageRoute = LTRIM(RTRIM(PageRoute)),
    Category = UPPER(LTRIM(RTRIM(Category))),
    PageName = LTRIM(RTRIM(PageName));

-- 2. Delete duplicates based on PageRoute
-- We keep the record with the LOWEST PageId to maintain existing permissions if possible
DELETE FROM AppPages 
WHERE PageId NOT IN (
    SELECT MIN(PageId) 
    FROM AppPages 
    GROUP BY PageRoute
);

-- 3. Force Category consistency for the Sidebar (Always ALL CAPS)
UPDATE AppPages SET Category = 'FINANCE' WHERE Category LIKE 'FINANCE%';
UPDATE AppPages SET Category = 'MEMBERSHIP' WHERE Category LIKE 'MEMBERSHIP%';
UPDATE AppPages SET Category = 'CONTROLLERS' WHERE Category LIKE 'CONTROLLERS%';
UPDATE AppPages SET Category = 'ADMINISTRATION' WHERE Category LIKE 'ADMIN%';
UPDATE AppPages SET Category = 'WEBSITE' WHERE Category LIKE 'WEBSITE%';
UPDATE AppPages SET Category = 'HALL RENTALS' WHERE Category LIKE 'HALL%';
UPDATE AppPages SET Category = 'SYSTEM' WHERE Category LIKE 'SYSTEM%';
UPDATE AppPages SET Category = 'CAMERA SYSTEM' WHERE Category LIKE 'CAMERA%';
UPDATE AppPages SET Category = 'GFC STUDIO' WHERE Category LIKE 'STUDIO%' OR Category LIKE 'GFC%';

-- 4. Specifically fix the "Financial Insights" entry
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/finance/insights')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Financial Insights', '/finance/insights', 'In-depth financial performance analysis', 'FINANCE', 0, 1, 36);
END
ELSE
BEGIN
    UPDATE AppPages SET 
        PageName = 'Financial Insights',
        Category = 'FINANCE',
        IsActive = 1,
        Description = 'In-depth financial performance analysis'
    WHERE PageRoute = '/finance/insights';
END

-- 5. Cleanup any stray pages that might be missing categories
UPDATE AppPages SET Category = 'ADMINISTRATION' WHERE PageRoute = '/users' AND Category IS NULL;
UPDATE AppPages SET Category = 'FINANCE' WHERE PageRoute LIKE '/reimbursements%' AND Category IS NULL;

GO
