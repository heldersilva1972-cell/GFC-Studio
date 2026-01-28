
USE ClubMembership;
GO

-- Helper to insert if not exists
CREATE PROCEDURE #EnsurePage
    @PageName NVARCHAR(100),
    @PageRoute NVARCHAR(100),
    @Category NVARCHAR(50),
    @Description NVARCHAR(255),
    @DisplayOrder INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = @PageRoute)
    BEGIN
        INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
        VALUES (@PageName, @PageRoute, @Description, @Category, 0, 1, @DisplayOrder);
        PRINT 'Inserted: ' + @PageName;
    END
    ELSE
    BEGIN
        -- Optional: Update category if it's currently NULL or "UNSPECIFIED" to ensure it shows up
        UPDATE AppPages 
        SET Category = @Category, IsActive = 1
        WHERE PageRoute = @PageRoute AND (Category IS NULL OR Category = 'UNSPECIFIED');
        PRINT 'Verified: ' + @PageName;
    END
END
GO

-- 1. Mobile Hub (Hidden, but needed for auto-grant)
EXEC #EnsurePage 'Mobile Portal Hub', '/mobile', 'MOBILE APPS', 'Main entry point for mobile tools', 0;

-- 2. Bartender Schedule
EXEC #EnsurePage 'Bartender Schedule', '/mobile/schedule', 'MOBILE APPS', 'View personal and team shifts', 10;

-- 3. Shift Reports
EXEC #EnsurePage 'Shift Reports', '/mobile/shift-report', 'MOBILE APPS', 'Submit end-of-shift sales data', 20;

-- 4. Liquor Checkout
EXEC #EnsurePage 'Liquor Checkout', '/mobile/liquor/checkout', 'MOBILE APPS', 'Scan bottles out of inventory', 30;

-- 5. Liquor Manager
EXEC #EnsurePage 'Liquor Manager', '/mobile/liquor/manage', 'MOBILE APPS', 'Full inventory management', 40;

-- 6. Mobile Analytics
EXEC #EnsurePage 'Mobile Analytics', '/mobile/analytics', 'MOBILE APPS', 'Revenue charts and performance', 50;

-- 7. Key Cards
EXEC #EnsurePage 'Key Cards', '/mobile/keycards', 'MOBILE APPS', 'Door access management', 60;

-- 8. Dues & Payments
EXEC #EnsurePage 'Dues & Payments', '/mobile/dues', 'MOBILE APPS', 'Member payment tracking', 70;

-- 9. Schedule Manager
EXEC #EnsurePage 'Schedule Manager', '/mobile/manage-schedule', 'MOBILE APPS', 'Admin tool for rosters', 80;

DROP PROCEDURE #EnsurePage;
GO
