-- Comprehensive Cleanup of AppPages table
-- Ensures categories and names match the expected structure

-- 1. Correct any pages that might be stuck with 'UNSPECIFIED' or NULL categories
UPDATE AppPages SET Category = 'Finance' WHERE PageRoute = '/finance/insights';
UPDATE AppPages SET Category = 'System' WHERE PageRoute = '/admin/system/communications';

-- 2. Normalize and ensure all pages are Active
UPDATE AppPages SET IsActive = 1 WHERE PageRoute IN ('/finance/insights', '/admin/system/communications');

-- 3. Ensure they have the correct names
UPDATE AppPages SET PageName = 'Financial Insights' WHERE PageRoute = '/finance/insights';
UPDATE AppPages SET PageName = 'Text & Email Setup' WHERE PageRoute = '/admin/system/communications';

-- 4. Double check all pages from the population script are actually there and have the right category
-- We'll use the routes to identify them
UPDATE AppPages SET Category = 'Finance' WHERE PageRoute IN ('/reimbursements', '/manage-reimbursements', '/reports', '/lottery-sales', '/bar-sales', '/bartender-shift');
UPDATE AppPages SET Category = 'Membership' WHERE PageRoute IN ('/members', '/dues', '/keycards', '/life-eligibility', '/non-portuguese-queue', '/bylaws', '/directors', '/physical-keys');
UPDATE AppPages SET Category = 'Controllers' WHERE PageRoute IN ('/controllers', '/search-controller', '/access-holidays');
UPDATE AppPages SET Category = 'Administration' WHERE PageRoute IN ('/users', '/security-policy', '/legal-documents', '/data-export', '/audit-logs', '/admin/hosting-security');
UPDATE AppPages SET Category = 'Website' WHERE PageRoute IN ('/page-management', '/review-moderation', '/event-promotions', '/nav-menu-editor', '/website-settings', '/form-builder', '/form-submissions', '/special-events');
UPDATE AppPages SET Category = 'Hall Rentals' WHERE PageRoute IN ('/rental-requests', '/rental-settings');
UPDATE AppPages SET Category = 'System' WHERE PageRoute IN ('/infrastructure-hub', '/migration-wizard');
UPDATE AppPages SET Category = 'Camera System' WHERE PageRoute IN ('/camera-monitor', '/camera-setup', '/camera-audit-log');
UPDATE AppPages SET Category = 'GFC Studio' WHERE PageRoute IN ('/visual-editor');

-- 5. Final check for '/finance/insights' existence just in case
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/finance/insights')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Financial Insights', '/finance/insights', 'In-depth financial performance analysis', 'Finance', 0, 1, 36);
END

GO
