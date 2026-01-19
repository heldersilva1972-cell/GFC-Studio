-- Register Mobile Analytics Page in AppPages
-- This allows the page to be assigned to users via the User Management interface

-- Check if the page already exists
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/mobile/analytics')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES (
        'Mobile Analytics',
        '/mobile/analytics',
        'Mobile',
        1,
        0,
        120,
        'Mobile analytics and performance dashboard'
    );
    
    PRINT 'Successfully registered /mobile/analytics page';
END
ELSE
BEGIN
    PRINT '/mobile/analytics page already exists';
END
GO
