-- Re-populate AppPages table to match the current GFC system sidebar exactly.
-- This script ensures the permissions modal matches the actual navigation.

-- 1. Reset everything to ensure a clean sync
UPDATE AppPages SET IsActive = 0;

-- Helper table for the actual pages
DECLARE @ActualPages TABLE (
    Name NVARCHAR(100),
    Route NVARCHAR(255),
    Category NVARCHAR(100),
    Descr NVARCHAR(500),
    IsAdmin BIT,
    DisplayOrder INT
);

INSERT INTO @ActualPages (Name, Route, Category, Descr, IsAdmin, DisplayOrder) VALUES
-- DASHBOARD
('Dashboard', '/', 'DASHBOARD', 'Main overview and stats', 0, 1),

-- MEMBERSHIP
('Members Info', '/members', 'MEMBERSHIP', 'View and manage member directory', 0, 10),
('Dues/Payments', '/dues', 'MEMBERSHIP', 'Manage member dues and history', 0, 11),
('Door Key Cards', '/keycards', 'MEMBERSHIP', 'Key card assignment and management', 0, 12),
('Life Eligibility', '/life-eligibility', 'MEMBERSHIP', 'Life membership tracking', 0, 13),
('Non-Portuguese Queue', '/np-queue', 'MEMBERSHIP', 'Waitlist management', 0, 14),
('Club Bylaws', '/bylaws', 'MEMBERSHIP', 'Digital bylaw access', 0, 15),
('Directors', '/directors', 'MEMBERSHIP', 'Current board of directors', 0, 16),
('Physical Keys', '/physicalkeys', 'MEMBERSHIP', 'Manual key inventory', 0, 17),

-- CONTROLLERS
('Access Controllers', '/controllers', 'CONTROLLERS', 'Access control system status', 1, 20),
('Search Controller', '/controllers/discovery', 'CONTROLLERS', 'Hardware discovery and logs', 1, 21),
('Access Holidays', '/controllers/schedules/holidays', 'CONTROLLERS', 'Holiday schedule overrides', 1, 22),

-- FINANCE
('Financial Insights', '/finance/insights', 'FINANCE', 'Advanced financial analytics', 1, 30),
('Reimbursements', '/reimbursements', 'FINANCE', 'Submit personal reimbursements', 0, 31),
('Manage Reimbursements', '/reimbursements/manage', 'FINANCE', 'Audit and approve requests', 1, 32),
('Reports', '/reimbursements/reports', 'FINANCE', 'Financial data exports', 1, 33),
('Lottery Sales', '/lottery', 'FINANCE', 'Lottery sales performance', 0, 34),
('Bar Sales Entry', '/admin/bar-sales', 'FINANCE', 'Register nightly bar sales', 1, 35),
('Bartender Schedule', '/admin/staff-shifts', 'FINANCE', 'Staffing and shifts', 1, 36),

-- ADMINISTRATION
('Manage Users', '/users', 'ADMINISTRATION', 'System account management', 1, 40),
('Live Activity Feed', '/admin/users/live-activity', 'ADMINISTRATION', 'Real-time system audit logs', 1, 41),
('Security & Access Hub', '/admin/security-settings', 'ADMINISTRATION', 'Security policies and permissions', 1, 42),
('Data Export', '/export', 'ADMINISTRATION', 'Global data backup and CSV export', 1, 43),

-- WEBSITE
('Page Management', '/admin/pages', 'WEBSITE', 'CMS page editor', 1, 50),
('Review Moderation', '/admin/reviews', 'WEBSITE', 'Public review oversight', 1, 51),
('Event Promotions', '/admin/event-promotions', 'WEBSITE', 'Landing page promotion blocks', 1, 52),
('Nav Menu Editor', '/admin/nav-menu-editor', 'WEBSITE', 'Main menu structure', 1, 53),
('Website Settings', '/admin/website-settings', 'WEBSITE', 'Global site metadata', 1, 54),
('Form Builder', '/admin/form-builder', 'WEBSITE', 'Interactive form design', 1, 55),
('Form Submissions', '/admin/form-submissions', 'WEBSITE', 'Form data viewing', 1, 56),
('Special Events', '/controllers/schedules/specialevents', 'WEBSITE', 'One-time event schedules', 1, 57),

-- HALL RENTALS
('Rental Requests', '/admin/hall-management', 'HALL RENTALS', 'Hall booking platform', 1, 60),
('Rental Settings', '/admin/hall-rental-settings', 'HALL RENTALS', 'Pricing and policies', 1, 61),

-- SYSTEM
('Infrastructure Hub', '/admin/operations', 'SYSTEM', 'Server health and recovery tools', 1, 70),
('Text & Email Setup', '/admin/system/communications', 'SYSTEM', 'Communication channel configuration', 1, 71),
('System Alert Registry', '/admin/system/alerts', 'SYSTEM', 'Configuration for automated alerts', 1, 72),

-- CAMERA SYSTEM
('View Monitor', '/cameras/view', 'CAMERA SYSTEM', 'Real-time NVR feeds', 0, 80),
('System Setup', '/cameras/configure', 'CAMERA SYSTEM', 'Camera node configuration', 1, 81),
('Audit Log', '/cameras/audit', 'CAMERA SYSTEM', 'Camera system event log', 1, 82),

-- GFC STUDIO
('Visual Editor', '/studio', 'GFC STUDIO', 'Premium visual design center', 0, 90);

-- Sync the temporary list to the real table
MERGE AppPages AS target
USING @ActualPages AS source
ON LOWER(target.PageRoute) = LOWER(source.Route)
WHEN MATCHED THEN
    UPDATE SET 
        PageName = source.Name,
        Category = source.Category,
        Description = source.Descr,
        RequiresAdmin = source.IsAdmin,
        IsActive = 1,
        DisplayOrder = source.DisplayOrder
WHEN NOT MATCHED THEN
    INSERT (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES (source.Name, source.Route, source.Descr, source.Category, source.IsAdmin, 1, source.DisplayOrder);

-- Final cleanup: Remove anything that wasn't reactivated
DELETE FROM AppPages WHERE IsActive = 0;

SELECT 'SIDEBAR SYNC COMPLETE. Database now matches actual navigation exactly.';
