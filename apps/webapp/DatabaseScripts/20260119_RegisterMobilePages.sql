-- Register Mobile Pages in AppPages table so they appear in User Management permissions
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/mobile')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('Staff Portal Hub', '/mobile', 'Mobile', 1, 0, 100, 'Main hub for mobile staff tools');
    PRINT 'Registered Staff Portal Hub page.';
END
ELSE
BEGIN
    PRINT 'Staff Portal Hub page already registered.';
END

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/mobile/shift-report')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('Shift Report', '/mobile/shift-report', 'Mobile', 1, 0, 110, 'Mobile shift and sales reporting form');
    PRINT 'Registered Shift Report page.';
END
ELSE
BEGIN
    PRINT 'Shift Report page already registered.';
END
GO
