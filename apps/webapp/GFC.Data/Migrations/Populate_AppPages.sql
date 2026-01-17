-- Populate AppPages table with all pages from the Control Center navigation
-- This matches the exact structure shown in the sidebar

-- Dashboard
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Dashboard', '/', 'Main dashboard', 'Dashboard', 0, 1, 1);

-- MEMBERSHIP
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/members')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Members Info', '/members', 'View and manage members', 'Membership', 0, 1, 10);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/dues')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Dues/Payments', '/dues', 'Manage member dues and payments', 'Membership', 0, 1, 11);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/keycards')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Door Key Cards', '/keycards', 'Key card management', 'Membership', 0, 1, 12);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/life-eligibility')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Life Eligibility', '/life-eligibility', 'Life membership eligibility', 'Membership', 0, 1, 13);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/non-portuguese-queue')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Non-Portuguese Queue', '/non-portuguese-queue', 'Non-Portuguese membership queue', 'Membership', 0, 1, 14);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/bylaws')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Club Bylaws', '/bylaws', 'Club bylaws and regulations', 'Membership', 0, 1, 15);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/directors')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Directors', '/directors', 'Board of directors', 'Membership', 0, 1, 16);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/physical-keys')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Physical Keys', '/physical-keys', 'Physical key management', 'Membership', 0, 1, 17);

-- CONTROLLERS
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/controllers')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Access Controllers', '/controllers', 'Access control system dashboard', 'Controllers', 0, 1, 20);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/search-controller')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Search Controller', '/search-controller', 'Search controller events', 'Controllers', 0, 1, 21);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/access-holidays')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Access Holidays', '/access-holidays', 'Configure access holidays', 'Controllers', 0, 1, 22);

-- FINANCE
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/reimbursements')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Reimbursements', '/reimbursements', 'Member reimbursements', 'Finance', 0, 1, 30);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/manage-reimbursements')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Manage Reimbursements', '/manage-reimbursements', 'Manage reimbursement requests', 'Finance', 0, 1, 31);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/reports')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Reports', '/reports', 'Financial reports', 'Finance', 0, 1, 32);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/lottery-sales')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Lottery Sales', '/lottery-sales', 'Lottery sales tracking', 'Finance', 0, 1, 33);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/bar-sales')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Bar Sales Entry', '/bar-sales', 'Bar sales entry', 'Finance', 0, 1, 34);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/bartender-shift')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Bartender Shift', '/bartender-shift', 'Bartender shift management', 'Finance', 0, 1, 35);

-- ADMINISTRATION
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/users')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Manage Users', '/users', 'User account management', 'Administration', 1, 1, 40);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/security-policy')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Security & Policy', '/security-policy', 'Security and policy settings', 'Administration', 1, 1, 41);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/legal-documents')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Legal & Documents', '/legal-documents', 'Legal documents management', 'Administration', 1, 1, 42);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/data-export')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Data Export', '/data-export', 'Export system data', 'Administration', 1, 1, 43);

-- WEBSITE
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/page-management')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Page Management', '/page-management', 'Manage website pages', 'Website', 0, 1, 50);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/review-moderation')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Review Moderation', '/review-moderation', 'Moderate user reviews', 'Website', 0, 1, 51);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/event-promotions')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Event Promotions', '/event-promotions', 'Promote events', 'Website', 0, 1, 52);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/nav-menu-editor')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Nav Menu Editor', '/nav-menu-editor', 'Edit navigation menu', 'Website', 0, 1, 53);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/website-settings')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Website Settings', '/website-settings', 'Configure website settings', 'Website', 0, 1, 54);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/form-builder')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Form Builder', '/form-builder', 'Build custom forms', 'Website', 0, 1, 55);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/form-submissions')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Form Submissions', '/form-submissions', 'View form submissions', 'Website', 0, 1, 56);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/special-events')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Special Events', '/special-events', 'Special events management', 'Website', 0, 1, 57);

-- HALL RENTALS
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/rental-requests')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Rental Requests', '/rental-requests', 'Manage hall rental requests', 'Hall Rentals', 0, 1, 60);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/rental-settings')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Settings', '/rental-settings', 'Hall rental settings', 'Hall Rentals', 0, 1, 61);

-- SYSTEM
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/infrastructure-hub')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Infrastructure Hub', '/infrastructure-hub', 'System infrastructure management', 'System', 1, 1, 70);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/migration-wizard')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Migration Wizard', '/migration-wizard', 'Data migration tools', 'System', 1, 1, 71);

-- CAMERA SYSTEM
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/camera-monitor')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('View Monitor', '/camera-monitor', 'View camera feeds', 'Camera System', 0, 1, 80);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/camera-setup')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('System Setup', '/camera-setup', 'Camera system configuration', 'Camera System', 1, 1, 81);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/camera-audit-log')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Audit Log', '/camera-audit-log', 'Camera system audit log', 'Camera System', 1, 1, 82);

-- GFC STUDIO
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/visual-editor')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Visual Editor', '/visual-editor', 'Visual page editor', 'GFC Studio', 0, 1, 90);

-- Additional common pages
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/admin/hosting-security')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Hosting & Security', '/admin/hosting-security', 'Hosting and security configuration', 'Administration', 1, 1, 44);

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/audit-logs')
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Audit Logs', '/audit-logs', 'System audit logs', 'Administration', 1, 1, 45);

SELECT 'Pages populated successfully. Total pages: ' + CAST(COUNT(*) AS VARCHAR) FROM AppPages;
