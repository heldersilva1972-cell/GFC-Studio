-- Add Mobile Key Cards and Mobile Dues pages to AppPages
-- Date: 2026-01-22
-- Purpose: Register new mobile pages for permission management

-- Check if pages already exist to avoid duplicates
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/mobile/keycards')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, Description, RequiresAdmin, IsActive, DisplayOrder)
    VALUES (
        'Mobile Key Cards',
        '/mobile/keycards',
        'Mobile',
        'Manage member key card assignments and status on mobile devices',
        0, -- Not admin-only, can be delegated
        1,
        100
    );
    PRINT 'Added Mobile Key Cards page';
END
ELSE
BEGIN
    PRINT 'Mobile Key Cards page already exists';
END

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/mobile/dues')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, Description, RequiresAdmin, IsActive, DisplayOrder)
    VALUES (
        'Mobile Dues & Payments',
        '/mobile/dues',
        'Mobile',
        'View and record membership dues and payments on mobile devices',
        0, -- Not admin-only, can be delegated
        1,
        101
    );
    PRINT 'Added Mobile Dues & Payments page';
END
ELSE
BEGIN
    PRINT 'Mobile Dues & Payments page already exists';
END

-- Verify the pages were added
SELECT 
    PageID,
    PageName,
    PageRoute,
    Category,
    RequiresAdmin,
    IsActive
FROM AppPages
WHERE PageRoute IN ('/mobile/keycards', '/mobile/dues')
ORDER BY PageRoute;

PRINT 'Mobile pages registration complete. These pages can now be assigned to users through User Management.';
