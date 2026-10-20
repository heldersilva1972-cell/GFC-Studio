-- ============================================================================
-- GFC CONNECT PERMISSION CLEANUP & REGISTRATION
-- Removes legacy placeholder items and registers the 3 active Connect App cards
-- ============================================================================

-- 1. Remove obsolete legacy placeholder cards and their user permission links
DELETE FROM UserPagePermissions 
WHERE PageId IN (
    SELECT PageId FROM AppPages 
    WHERE PageRoute IN (
        '/gfc-connect/access',        -- Master Access (Not needed; enrollment grants app entry)
        '/gfc-connect/door-access',   -- Door Access (Placeholder)
        '/gfc-connect/shift-reports', -- Shift Reports (Mobile Portal, not Connect App)
        '/gfc-connect/club-news',     -- Club News (Placeholder)
        '/gfc-connect/member-dues'    -- Member Dues (Placeholder)
    )
);

DELETE FROM AppPages 
WHERE PageRoute IN (
    '/gfc-connect/access',
    '/gfc-connect/door-access',
    '/gfc-connect/shift-reports',
    '/gfc-connect/club-news',
    '/gfc-connect/member-dues'
);

-- 2. Ensure the 3 actual GFC Connect feature cards are registered

-- Card 1: Rental Bookings & Approvals
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/gfc-connect/hall-rentals')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('Rental Bookings & Approvals', '/gfc-connect/hall-rentals', 'GFC CONNECT', 1, 0, 1010, 'Manage inquiries, approve requests, and record payments in GFC Connect');
END
ELSE
BEGIN
    UPDATE AppPages 
    SET PageName = 'Rental Bookings & Approvals',
        Category = 'GFC CONNECT',
        IsActive = 1,
        DisplayOrder = 1010,
        Description = 'Manage inquiries, approve requests, and record payments in GFC Connect'
    WHERE PageRoute = '/gfc-connect/hall-rentals';
END

-- Card 2: Hall Rental Calendar
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/gfc-connect/rental-calendar')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('Hall Rental Calendar', '/gfc-connect/rental-calendar', 'GFC CONNECT', 1, 0, 1020, 'View bookings, club events, and hall availability in GFC Connect');
END
ELSE
BEGIN
    UPDATE AppPages 
    SET PageName = 'Hall Rental Calendar',
        Category = 'GFC CONNECT',
        IsActive = 1,
        DisplayOrder = 1020,
        Description = 'View bookings, club events, and hall availability in GFC Connect'
    WHERE PageRoute = '/gfc-connect/rental-calendar';
END

-- Card 3: Lottery Shifts & Variances
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/gfc-connect/lottery-shifts')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('Lottery Shifts', '/gfc-connect/lottery-shifts', 'GFC CONNECT', 1, 0, 1030, 'Active variance alerts, shift breakdowns, and audit history in GFC Connect');
END
ELSE
BEGIN
    UPDATE AppPages 
    SET PageName = 'Lottery Shifts',
        Category = 'GFC CONNECT',
        IsActive = 1,
        DisplayOrder = 1030,
        Description = 'Active variance alerts, shift breakdowns, and audit history in GFC Connect'
    WHERE PageRoute = '/gfc-connect/lottery-shifts';
END

PRINT 'GFC Connect permissions cleaned up and registered successfully.';
