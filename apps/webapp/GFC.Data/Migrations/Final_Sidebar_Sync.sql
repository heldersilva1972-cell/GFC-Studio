-- FINAL SIDEBAR SYNCHRONIZATION
-- This script performs a destructive sync to ensure the AppPages table matches the UI exactly.

-- 1. Reset all pages to inactive
UPDATE AppPages SET IsActive = 0;

-- 2. Define the current master page list
DECLARE @Pages TABLE (
    Name NVARCHAR(100),
    Route NVARCHAR(255),
    Category NVARCHAR(100),
    Descr NVARCHAR(500),
    IsAdmin BIT,
    Ord INT
);

INSERT INTO @Pages (Name, Route, Category, Descr, IsAdmin, Ord) VALUES
-- DASHBOARD
('Dashboard', '/', 'DASHBOARD', 'Main overview', 0, 1),

-- MEMBERSHIP
('Members Info', '/members', 'MEMBERSHIP', 'Member directory', 0, 10),
('Dues/Payments', '/dues', 'MEMBERSHIP', 'Payments and history', 0, 11),
('Door Key Cards', '/keycards', 'MEMBERSHIP', 'Access tokens', 0, 12),
('Life Eligibility', '/life-eligibility', 'MEMBERSHIP', 'Life tracking', 0, 13),
('Non-Portuguese Queue', '/np-queue', 'MEMBERSHIP', 'Waitlist', 0, 14),
('Club Bylaws', '/bylaws', 'MEMBERSHIP', 'Regulations', 0, 15),
('Directors', '/directors', 'MEMBERSHIP', 'Board of directors', 0, 16),
('Physical Keys', '/physicalkeys', 'MEMBERSHIP', 'Key inventory', 0, 17),

-- CONTROLLERS
('Access Controllers', '/controllers', 'CONTROLLERS', 'Hardware status', 1, 20),
('Search Controller', '/controllers/discovery', 'CONTROLLERS', 'Discovery tools', 1, 21),
('Access Holidays', '/controllers/schedules/holidays', 'CONTROLLERS', 'Holiday overrides', 1, 22),

-- FINANCE
('Financial Insights', '/finance/insights', 'FINANCE', 'Analytics', 1, 30),
('Reimbursements', '/reimbursements', 'FINANCE', 'Submit personal', 0, 31),
('Manage Reimbursements', '/reimbursements/manage', 'FINANCE', 'Auditing', 1, 32),

('Lottery Sales', '/lottery', 'FINANCE', 'Lottery data', 0, 34),
('Bar Sales Entry', '/admin/bar-sales', 'FINANCE', 'Nightly sales', 1, 35),
('Bartender Schedule', '/admin/staff-shifts', 'MEMBERSHIP', 'Staffing', 1, 36),

-- ADMINISTRATION
('Manage Users', '/users', 'ADMINISTRATION', 'User accounts', 1, 40),
('Live Activity Feed', '/admin/users/live-activity', 'ADMINISTRATION', 'System logs', 1, 41),
('Security & Access Hub', '/admin/security-settings', 'ADMINISTRATION', 'Security policies', 1, 42),
('Data Export', '/export', 'ADMINISTRATION', 'CSV exports', 1, 43),

-- WEBSITE
('Page Management', '/admin/pages', 'WEBSITE', 'Website CMS', 1, 50),
('Review Moderation', '/admin/reviews', 'WEBSITE', 'Review oversight', 1, 51),
('Event Promotions', '/admin/event-promotions', 'WEBSITE', 'Landing promos', 1, 52),
('Nav Menu Editor', '/admin/nav-menu-editor', 'WEBSITE', 'Menu structure', 1, 53),
('Website Settings', '/admin/website-settings', 'WEBSITE', 'Site metadata', 1, 54),
('Form Builder', '/admin/form-builder', 'WEBSITE', 'Build forms', 1, 55),
('Form Submissions', '/admin/form-submissions', 'WEBSITE', 'View submissions', 1, 56),
('Special Events', '/controllers/schedules/specialevents', 'WEBSITE', 'Event schedules', 1, 57),

-- HALL RENTALS
('Rental Requests', '/admin/hall-management', 'HALL RENTALS', 'Hall rentals', 1, 60),
('Rental Settings', '/admin/hall-rental-settings', 'HALL RENTALS', 'Policies', 1, 61),

-- SYSTEM
('Infrastructure Hub', '/admin/operations', 'SYSTEM', 'Server health', 1, 70),
('Text & Email Setup', '/admin/system/communications', 'SYSTEM', 'Communications', 1, 71),
('System Alert Registry', '/admin/system/alerts', 'SYSTEM', 'Alert configs', 1, 72),

-- CAMERA SYSTEM
('View Monitor', '/cameras/view', 'CAMERA SYSTEM', 'Real-time feeds', 0, 80),
('System Setup', '/cameras/configure', 'CAMERA SYSTEM', 'Camera setup', 1, 81),
('Audit Log', '/cameras/audit', 'CAMERA SYSTEM', 'System logs', 1, 82),

-- GFC STUDIO
('Visual Editor', '/studio', 'GFC STUDIO', 'Visual design', 0, 90),

-- MOBILE APPS
('Mobile Hub', '/mobile', 'MOBILE APPS', 'Mobile terminal', 0, 100),
('Mobile Schedule', '/mobile/schedule', 'MOBILE APPS', 'Bartender shifts', 0, 101),
('Mobile Sales', '/mobile/shift-report', 'MOBILE APPS', 'End of shift', 0, 102),
('Mobile Checkout', '/mobile/liquor/checkout', 'MOBILE APPS', 'Bottle scanning', 0, 103),
('Liquor Hub', '/mobile/liquor/hub', 'MOBILE APPS', 'Central liquor management', 0, 104),
('Mobile Inventory', '/mobile/liquor/manage', 'MOBILE APPS', 'Stock control', 0, 105),
('Mobile Analytics', '/mobile/analytics', 'MOBILE APPS', 'Income charts', 0, 106),
('Mobile Key Cards', '/mobile/keycards', 'MOBILE APPS', 'Direct access', 0, 107),
('Mobile Dues', '/mobile/dues', 'MOBILE APPS', 'Payment entry', 0, 108),
('Mobile Roster', '/mobile/manage-schedule', 'MOBILE APPS', 'Schedule admin', 0, 109),
('User Management', '/mobile/users', 'MOBILE APPS', 'Mobile user control', 0, 110),
('Submit Reimbursement', '/mobile/reimbursements', 'MOBILE APPS', 'Expense submission', 0, 111),
('Reimbursement Hub', '/mobile/reimbursements/manager', 'MOBILE APPS', 'Payout hub', 0, 112),
('System Vitals', '/mobile/system/stats', 'MOBILE APPS', 'Server stats', 0, 113),
('Sales Alert Widget', 'widgets/shift-sales-alert', 'MOBILE APPS', 'Shift alerts', 0, 114);

-- 3. Perform the Merge
MERGE AppPages AS target
USING @Pages AS source
ON LOWER(target.PageRoute) = LOWER(source.Route)
WHEN MATCHED THEN
    UPDATE SET 
        PageName = source.Name,
        Category = source.Category,
        Description = source.Descr,
        RequiresAdmin = source.IsAdmin,
        IsActive = 1,
        DisplayOrder = source.Ord
WHEN NOT MATCHED THEN
    INSERT (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES (source.Name, source.Route, source.Descr, source.Category, source.IsAdmin, 1, source.Ord);

-- 4. Delete orphaned entries
DELETE FROM AppPages WHERE IsActive = 0;

SELECT 'SYNC COMPLETE: The database now matches the sidebar exactly.';
