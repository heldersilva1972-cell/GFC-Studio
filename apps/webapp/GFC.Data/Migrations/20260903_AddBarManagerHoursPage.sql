-- Migration: Place Bar Manager Hours directly into MOBILE APPS
-- Date: 2026-09-03

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AppPages')
BEGIN
    -- Ensure all mobile pages belong to the single unified 'MOBILE APPS' category
    IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/mobile/bar-manager-hours')
    BEGIN
        INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
        VALUES ('Bar Manager Hours (Mobile)', '/mobile/bar-manager-hours', 'Record daily bar manager working hours', 'MOBILE APPS', 0, 1, 11);
        PRINT 'Inserted Bar Manager Hours into MOBILE APPS.';
    END
    ELSE
    BEGIN
        UPDATE AppPages
        SET PageName = 'Bar Manager Hours (Mobile)',
            Category = 'MOBILE APPS',
            IsActive = 1
        WHERE PageRoute = '/mobile/bar-manager-hours';
        PRINT 'Updated Bar Manager Hours Category to MOBILE APPS.';
    END

    -- Clean up any lingering GFC PORTAL (MOBILE) categories to MOBILE APPS
    UPDATE AppPages
    SET Category = 'MOBILE APPS'
    WHERE Category = 'GFC PORTAL (MOBILE)';
END
GO
