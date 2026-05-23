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
('Dashboard', '/', 'GENERAL', 'Main overview', 0, 1),

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
('Access Holidays', '/controllers/holidays', 'CONTROLLERS', 'Holiday overrides', 1, 21),
('Search Controller', '/controllers/discovery', 'CONTROLLERS', 'Discovery tools', 1, 22),

-- FINANCE
('Financial Overview', '/finance/overview', 'FINANCE', 'Financial Overview Dashboard', 1, 30),
('Financial Insights', '/finance/insights', 'FINANCE', 'Analytics', 1, 31),
('Bills & Invoices', '/finance/bills', 'FINANCE', 'Manage Bills & Invoices', 1, 32),
('Reimbursements', '/reimbursements', 'FINANCE', 'Submit personal', 0, 33),
('Lottery Sales', '/lottery', 'FINANCE', 'Lottery data', 0, 34),
('Lottery Analytics', '/finance/lottery-analytics', 'FINANCE', 'Performance trends', 1, 35),
('Lottery Summaries', '/finance/lottery-summaries', 'FINANCE', 'Daily reports', 1, 36),
('Lottery Reconcile', '/finance/lottery-reconcile', 'FINANCE', 'Audit matching', 1, 37),
('Manage Reimbursements', '/reimbursements/manage', 'FINANCE', 'Auditing', 1, 38),
('Bar Sales Entry', '/admin/bar-sales', 'FINANCE', 'Nightly sales', 1, 39),
('Bar Sales', '/finance/bar-sales', 'FINANCE', 'Shift-based reports', 1, 40),
('Bar/Lottery Entries', '/finance/bar-lottery-sales', 'FINANCE', 'Sales records', 1, 41),
('Club Events Financials', '/finance/club-events', 'FINANCE', 'Event profit/loss', 1, 42),
('Employee Hours Worked', '/finance/employee-hours', 'FINANCE', 'Staff hours audit', 1, 43),
('Payroll Center', '/finance/payroll-center', 'FINANCE', 'Club Payroll Center', 1, 44),

-- ADMINISTRATION
('Manage Users', '/users', 'ADMINISTRATION', 'User accounts', 1, 50),
('Live Activity Feed', '/admin/users/live-activity', 'ADMINISTRATION', 'System logs', 1, 51),
('Active Device Sessions', '/admin/users/active-sessions', 'ADMINISTRATION', 'Live sessions', 1, 52),
('Security & Access Hub', '/admin/security-settings', 'ADMINISTRATION', 'Security policies', 1, 53),
('Administrative Logs', '/admin/audit-logs', 'ADMINISTRATION', 'Audit trail', 1, 54),
('Data Export', '/export', 'ADMINISTRATION', 'CSV exports', 1, 55),

-- WEBSITE
('Page Management', '/admin/pages', 'WEBSITE', 'Website CMS', 1, 60),
('Review Moderation', '/admin/reviews', 'WEBSITE', 'Review oversight', 1, 61),
('Event Promotions', '/admin/event-promotions', 'WEBSITE', 'Landing promos', 1, 62),
('Nav Menu Editor', '/admin/nav-menu-editor', 'WEBSITE', 'Menu structure', 1, 63),
('Website Settings', '/admin/website-settings', 'WEBSITE', 'Site metadata', 1, 64),
('Form Builder', '/admin/form-builder', 'WEBSITE', 'Build forms', 1, 65),
('Form Submissions', '/admin/form-submissions', 'WEBSITE', 'View submissions', 1, 66),
('Special Events', '/controllers/schedules/specialevents', 'WEBSITE', 'Event schedules', 1, 67),

-- HALL RENTALS
('Rental Requests', '/admin/hall-management', 'HALL RENTALS', 'Hall rentals', 1, 70),
('Rental Settings', '/admin/hall-rental-settings', 'HALL RENTALS', 'Policies', 1, 71),

-- SYSTEM
('Infrastructure Hub', '/admin/operations', 'SYSTEM', 'Server health', 1, 80),
('Text & Email Setup', '/admin/system/communications', 'SYSTEM', 'Communications', 1, 81),
('System Alert Registry', '/admin/system/alerts', 'SYSTEM', 'Alert configs', 1, 82),

-- CAMERA SYSTEM
('View Monitor', '/cameras/view', 'CAMERA SYSTEM', 'Real-time feeds', 0, 90),
('System Setup', '/cameras/configure', 'CAMERA SYSTEM', 'Camera setup', 1, 91),
('Audit Log', '/cameras/audit', 'CAMERA SYSTEM', 'System logs', 1, 92),

-- GFC STUDIO
('Visual Editor', '/studio', 'GFC STUDIO', 'Visual design', 0, 100),

-- BARTENDERS
('End of Shift Sales', '/operations/end-of-shift', 'BARTENDERS', 'Desktop shift entry', 0, 110),
('Bartender Schedule', '/admin/staff-shifts', 'BARTENDERS', 'Staffing', 1, 111),
('Schedule Manager', '/mobile/manage-schedule', 'BARTENDERS', 'Schedule admin', 1, 112),

-- MOBILE APPS
('Mobile Hub', '/mobile', 'MOBILE APPS', 'Mobile terminal', 0, 120),
('Mobile Schedule', '/mobile/schedule', 'MOBILE APPS', 'Bartender shifts', 0, 121),
('Mobile Sales', '/mobile/shift-report', 'MOBILE APPS', 'End of shift', 0, 122),
('Mobile Checkout', '/mobile/liquor/checkout', 'MOBILE APPS', 'Bottle scanning', 0, 123),
('Liquor Hub', '/mobile/liquor/hub', 'MOBILE APPS', 'Central liquor management', 0, 124),
('Mobile Inventory', '/mobile/liquor/manage', 'MOBILE APPS', 'Stock control', 0, 125),
('Mobile Analytics', '/mobile/analytics', 'MOBILE APPS', 'Income charts', 0, 126),
('Mobile Key Cards', '/mobile/keycards', 'MOBILE APPS', 'Direct access', 0, 127),
('Mobile Dues', '/mobile/dues', 'MOBILE APPS', 'Payment entry', 0, 128),
('User Management', '/mobile/users', 'MOBILE APPS', 'Mobile user control', 0, 129),
('Submit Reimbursement', '/mobile/reimbursements', 'MOBILE APPS', 'Expense submission', 0, 130),
('Reimbursement Hub', '/mobile/reimbursements/manager', 'MOBILE APPS', 'Payout hub', 0, 131),
('System Vitals', '/mobile/system/stats', 'MOBILE APPS', 'Server stats', 0, 132),
('Sales Alert Widget', 'widgets/shift-sales-alert', 'MOBILE APPS', 'Shift alerts', 0, 133),

-- BINGO
('Bingo Reports', '/admin/bingo', 'BINGO', 'Financial reports', 0, 140),
('Bingo Expenses', '/admin/bingo/expenses', 'BINGO', 'Expense tracking', 0, 141),
('Bingo Setup', '/admin/bingo/setup', 'BINGO', 'Configure games', 0, 142),
('Progressive Ball Counts', '/admin/bingo/progressive', 'BINGO', 'Progressive tracking', 0, 143),

-- POS SYSTEM
('POS Terminal', '/admin/pos-terminal', 'POS SYSTEM', 'POS Terminal interface', 1, 150),
('Sales Audit', '/admin/pos-audit', 'POS SYSTEM', 'Audit POS sales', 1, 151),
('Sales Analysis', '/admin/pos-analysis', 'POS SYSTEM', 'Analyze sales performance', 1, 152),
('POS Menu Manager', '/admin/pos-menu', 'POS SYSTEM', 'Manage POS menu items', 1, 153),
('Liquor Hub', '/liquor/hub', 'POS SYSTEM', 'Liquor database management', 1, 154);

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
