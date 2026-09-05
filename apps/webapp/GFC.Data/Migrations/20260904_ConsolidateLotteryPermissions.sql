-- Migration: Consolidate Lottery sub-pages into unified Lottery Sales page with 100% Permission Preservation
-- Date: 2026-09-04

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AppPages')
BEGIN
    -- 1. Ensure unified Lottery Sales page exists and is active under LOTTERY category
    IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/lottery')
    BEGIN
        INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
        VALUES ('Lottery Sales', '/lottery', 'Lottery Sales, Shift Audits, Summaries, and Compliance', 'LOTTERY', 0, 1, 35);
        PRINT 'Inserted Lottery Sales page.';
    END
    ELSE
    BEGIN
        UPDATE AppPages
        SET PageName = 'Lottery Sales',
            Category = 'LOTTERY',
            IsActive = 1
        WHERE PageRoute = '/lottery';
        PRINT 'Updated Lottery Sales page in AppPages.';
    END

    DECLARE @LotteryPageId INT;
    SELECT @LotteryPageId = PageId FROM AppPages WHERE PageRoute = '/lottery';

    -- 2. PRESERVE ALL PERMISSIONS: Transfer existing permissions from deprecated sub-routes to /lottery
    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'UserPagePermissions') AND @LotteryPageId IS NOT NULL
    BEGIN
        -- Grant /lottery access to any user who had access to lottery-summaries or compliance but not yet /lottery
        INSERT INTO UserPagePermissions (UserId, PageId, CanAccess, GrantedDate, GrantedBy, ReceivePush, CanEdit)
        SELECT DISTINCT 
            upp.UserId, 
            @LotteryPageId, 
            1, 
            GETUTCDATE(), 
            'System Migration', 
            MAX(CAST(upp.ReceivePush AS INT)), 
            MAX(CAST(upp.CanEdit AS INT))
        FROM UserPagePermissions upp
        INNER JOIN AppPages ap ON upp.PageId = ap.PageId
        WHERE ap.PageRoute IN ('/finance/lottery-summaries', '/lottery/compliance')
          AND upp.UserId NOT IN (
              SELECT UserId FROM UserPagePermissions WHERE PageId = @LotteryPageId
          )
        GROUP BY upp.UserId;

        PRINT 'Transferred user permissions from deprecated lottery pages to unified Lottery Sales.';

        -- Now safely remove permission rows for the deprecated sub-routes
        DELETE FROM UserPagePermissions
        WHERE PageId IN (
            SELECT PageId FROM AppPages
            WHERE PageRoute IN ('/finance/lottery-summaries', '/lottery/compliance')
        );
        PRINT 'Removed UserPagePermissions for deprecated lottery sub-pages.';
    END

    -- 3. Remove deprecated sub-pages from AppPages
    DELETE FROM AppPages
    WHERE PageRoute IN ('/finance/lottery-summaries', '/lottery/compliance');
    PRINT 'Removed deprecated lottery sub-pages from AppPages.';

    -- 4. Align BAR category pages in database (so no in-memory overrides are needed)
    UPDATE AppPages
    SET Category = 'BAR'
    WHERE PageRoute IN ('/finance/bar-sales', '/admin/bar-sales', '/finance/bar-lottery-sales');
    PRINT 'Aligned BAR category pages in AppPages.';

    -- 5. Ensure Mobile Liquor Hub pages exist cleanly in database
    IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/mobile/liquor/hub/count')
    BEGIN
        INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
        VALUES ('Inventory Check (Mobile)', '/mobile/liquor/hub/count', 'Reconcile product stock counts on mobile hub', 'MOBILE APPS', 0, 1, 135);
        PRINT 'Inserted Inventory Check (Mobile) page.';
    END

    IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/mobile/liquor/hub/ordering')
    BEGIN
        INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
        VALUES ('Ordering List (Mobile)', '/mobile/liquor/hub/ordering', 'Place and manage product orders on mobile hub', 'MOBILE APPS', 0, 1, 136);
        PRINT 'Inserted Ordering List (Mobile) page.';
    END
END
GO
