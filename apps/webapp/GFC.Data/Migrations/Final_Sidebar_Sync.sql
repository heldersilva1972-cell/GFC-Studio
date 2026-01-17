-- First, ensure the DefaultPermissions table exists for the new system
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DefaultPermissions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DefaultPermissions] (
        [PageId] INT NOT NULL,
        CONSTRAINT [PK_DefaultPermissions] PRIMARY KEY CLUSTERED ([PageId] ASC),
        CONSTRAINT [FK_DefaultPermissions_AppPages] FOREIGN KEY ([PageId]) REFERENCES [dbo].[AppPages] ([PageId]) ON DELETE CASCADE
    );
END

-- First, set all current pages to inactive so we can rebuild accurately
UPDATE AppPages SET IsActive = 0;

-- Helper to Insert or Update a page
DECLARE @Pages TABLE (
    Name NVARCHAR(100),
    Route NVARCHAR(255),
    Category NVARCHAR(100),
    Descr NVARCHAR(500),
    IsAdmin BIT
);

INSERT INTO @Pages (Name, Route, Category, Descr, IsAdmin) VALUES
-- MEMBERSHIP
('Members Info', '/members', 'MEMBERSHIP', 'Member management', 0),
('Dues/Payments', '/dues', 'MEMBERSHIP', 'Member dues and payments', 0),
('Door Key Cards', '/keycards', 'MEMBERSHIP', 'Key card management', 0),
('Life Eligibility', '/life-eligibility', 'MEMBERSHIP', 'Life membership eligibility', 0),
('Non-Portuguese Queue', '/np-queue', 'MEMBERSHIP', 'Non-Portuguese membership queue', 0),
('Club Bylaws', '/bylaws', 'MEMBERSHIP', 'Club bylaws and regulations', 0),
('Directors', '/directors', 'MEMBERSHIP', 'Board of directors management', 0),
('Physical Keys', '/physicalkeys', 'MEMBERSHIP', 'Physical key management', 0),

-- CONTROLLERS
('Access Controllers', '/controllers', 'CONTROLLERS', 'Access control system dashboard', 1),
('Search Controller', '/controllers/discovery', 'CONTROLLERS', 'Search and discovery', 1),
('Access Holidays', '/controllers/schedules/holidays', 'CONTROLLERS', 'Holiday access configuration', 1),

-- FINANCE
('Reimbursements', '/reimbursements', 'FINANCE', 'Member reimbursements', 0),
('Manage Reimbursements', '/reimbursements/manage', 'FINANCE', 'Financial management', 1),
('Reports', '/reimbursements/reports', 'FINANCE', 'Financial reports', 1),
('Lottery Sales', '/lottery', 'FINANCE', 'Lottery sales tracking', 0),
('Bar Sales Entry', '/admin/bar-sales', 'FINANCE', 'Bar sales entry', 0),
('Bartender Shift', '/bartender-shift', 'FINANCE', 'Shift management', 0),

-- ADMINISTRATION
('Manage Users', '/users', 'ADMINISTRATION', 'User account management', 1),
('Security & Policy', '/admin/security-settings', 'ADMINISTRATION', 'Security and policy settings', 1),
('Legal & Documents', '/admin/documents', 'ADMINISTRATION', 'Legal documents management', 1),
('Data Export', '/export', 'ADMINISTRATION', 'Export system data', 1),

-- WEBSITE
('Page Management', '/admin/pages', 'WEBSITE', 'Website page management', 1),
('Review Moderation', '/admin/reviews', 'WEBSITE', 'Moderate user reviews', 1),
('Event Promotions', '/admin/event-promotions', 'WEBSITE', 'Promote events', 1),
('Nav Menu Editor', '/admin/nav-menu-editor', 'WEBSITE', 'Edit navigation menu', 1),
('Website Settings', '/admin/website-settings', 'WEBSITE', 'Configure website settings', 1),
('Form Builder', '/admin/form-builder', 'WEBSITE', 'Build custom forms', 1),
('Form Submissions', '/admin/form-submissions', 'WEBSITE', 'View form submissions', 1),
('Special Events', '/controllers/schedules/specialevents', 'WEBSITE', 'Special events management', 1),

-- HALL RENTALS
('Rental Requests', '/admin/hall-management', 'HALL RENTALS', 'Manage hall rental requests', 1),
('Settings', '/admin/hall-rental-settings', 'HALL RENTALS', 'Hall rental settings', 1),

-- SYSTEM
('Infrastructure Hub', '/admin/operations', 'SYSTEM', 'System infrastructure management', 1),
('Migration Wizard', '/admin/system/migration', 'SYSTEM', 'Data migration tools', 1),

-- CAMERA SYSTEM
('View Monitor', '/cameras/view', 'CAMERA SYSTEM', 'View camera feeds', 0),
('System Setup', '/cameras/configure', 'CAMERA SYSTEM', 'Camera system configuration', 1),
('Audit Log', '/cameras/audit', 'CAMERA SYSTEM', 'Camera system audit log', 1),

-- GFC STUDIO
('Visual Editor', '/studio', 'GFC STUDIO', 'Visual page editor', 0);

-- MERGE INTO AppPages
MERGE AppPages AS target
USING @Pages AS source
ON LOWER(target.PageRoute) = LOWER(source.Route)
WHEN MATCHED THEN
    UPDATE SET 
        PageName = source.Name,
        Category = source.Category,
        Description = source.Descr,
        RequiresAdmin = source.IsAdmin,
        IsActive = 1
WHEN NOT MATCHED THEN
    INSERT (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES (source.Name, source.Route, source.Descr, source.Category, source.IsAdmin, 1, 0);

-- Final cleanup: Ensure naming consistency again for any stragglers
UPDATE AppPages SET Category = 'MEMBERSHIP' WHERE Category IN ('MEMBERS', 'Member');
UPDATE AppPages SET Category = 'ADMINISTRATION' WHERE Category = 'ADMIN';

-- Remove any old pages that were not in our list
DELETE FROM AppPages WHERE IsActive = 0;

SELECT 'SYNC COMPLETE: Database now matches sidebar exactly.';
