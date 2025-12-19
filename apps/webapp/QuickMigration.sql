-- Quick Migration Script for ClubMembership Database
-- Run this in SSMS or Azure Data Studio

USE ClubMembership;
GO

-- Create AppPages table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AppPages')
BEGIN
    CREATE TABLE AppPages (
        PageId INT IDENTITY(1,1) PRIMARY KEY,
        PageName NVARCHAR(100) NOT NULL,
        PageRoute NVARCHAR(200) NOT NULL UNIQUE,
        Description NVARCHAR(500) NULL,
        Category NVARCHAR(50) NULL,
        RequiresAdmin BIT NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1,
        DisplayOrder INT NOT NULL DEFAULT 0
    );
    PRINT 'Created AppPages table';
END
ELSE
BEGIN
    PRINT 'AppPages table already exists';
END
GO

-- Create UserPagePermissions table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserPagePermissions')
BEGIN
    CREATE TABLE UserPagePermissions (
        PermissionId INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        PageId INT NOT NULL,
        CanAccess BIT NOT NULL DEFAULT 1,
        GrantedDate DATETIME NOT NULL DEFAULT GETDATE(),
        GrantedBy NVARCHAR(100) NULL,
        
        CONSTRAINT FK_UserPagePermissions_Users FOREIGN KEY (UserId) 
            REFERENCES Users(UserId) ON DELETE CASCADE,
        CONSTRAINT FK_UserPagePermissions_Pages FOREIGN KEY (PageId) 
            REFERENCES AppPages(PageId) ON DELETE CASCADE,
        CONSTRAINT UQ_UserPagePermissions UNIQUE (UserId, PageId)
    );
    
    CREATE INDEX IX_UserPagePermissions_UserId ON UserPagePermissions(UserId);
    CREATE INDEX IX_UserPagePermissions_PageId ON UserPagePermissions(PageId);
    
    PRINT 'Created UserPagePermissions table';
END
ELSE
BEGIN
    PRINT 'UserPagePermissions table already exists';
END
GO

-- Insert default pages
IF NOT EXISTS (SELECT * FROM AppPages WHERE PageRoute = '/')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, DisplayOrder)
    VALUES 
        -- Dashboard & Main
        ('Dashboard', '/', 'Main dashboard', 'Main', 0, 1),
        ('Simulation Dashboard', '/simulation', 'Simulation control panel', 'Simulation', 0, 2),
        
        -- Members
        ('Members', '/members', 'Member management', 'Members', 0, 10),
        ('Member Detail', '/member/{id}', 'Member details page', 'Members', 0, 11),
        ('Directors', '/directors', 'Board of directors management', 'Members', 0, 12),
        ('Life Eligibility', '/life-eligibility', 'Life membership eligibility', 'Members', 0, 13),
        
        -- Access Control
        ('Key Cards', '/keycards', 'Key card management', 'Access Control', 0, 20),
        ('Physical Keys', '/physical-keys', 'Physical key management', 'Access Control', 0, 21),
        ('Member Access Control', '/member-access', 'Member access permissions', 'Access Control', 0, 22),
        
        -- Controllers
        ('Controllers Dashboard', '/controllers', 'Access control system dashboard', 'Controllers', 0, 30),
        ('Controller Status', '/controller-status', 'Controller status monitoring', 'Controllers', 0, 31),
        ('Controller Events', '/controller-events', 'Controller event logs', 'Controllers', 0, 32),
        ('Access Admin', '/controllers/access-admin', 'Access administration', 'Controllers', 0, 33),
        ('Door Configuration', '/controllers/door-config', 'Door configuration', 'Controllers', 0, 34),
        ('Network Config', '/controllers/network-config', 'Network configuration', 'Controllers', 0, 35),
        ('Schedules', '/controllers/schedules', 'Access schedules', 'Controllers', 0, 36),
        ('Auto Open', '/controllers/auto-open', 'Auto-open configuration', 'Controllers', 0, 37),
        ('Advanced Modes', '/controllers/advanced-modes', 'Advanced controller modes', 'Controllers', 0, 38),
        
        -- Financial
        ('Dues', '/dues', 'Membership dues management', 'Financial', 0, 40),
        ('Lottery', '/lottery', 'Lottery management', 'Financial', 0, 41),
        ('NP Queue', '/np-queue', 'Non-profit queue', 'Financial', 0, 42),
        ('Reimbursements', '/reimbursements', 'Reimbursement requests', 'Financial', 0, 43),
        ('Reimbursement Management', '/reimbursements/manage', 'Manage reimbursements', 'Financial', 0, 44),
        ('Reimbursement Reports', '/reimbursements/reports', 'Reimbursement reports', 'Financial', 0, 45),
        ('Reimbursement Settings', '/reimbursements/settings', 'Reimbursement settings', 'Financial', 1, 46),
        
        -- Administration
        ('User Management', '/users', 'User account management', 'Administration', 1, 50),
        ('Data Export', '/data-export', 'Export system data', 'Administration', 1, 51),
        ('Settings', '/settings', 'System settings', 'Administration', 1, 52),
        ('Audit Logs', '/admin/audit-logs', 'System audit logs', 'Administration', 1, 53),
        ('Notification Preferences', '/admin/notification-preferences', 'Notification settings', 'Administration', 1, 54),
        ('System Status', '/maintenance/system-status', 'System status and health', 'Administration', 1, 55),
        
        -- Simulation
        ('Card Reader Emulator', '/simulation/card-reader', 'Simulate card reader', 'Simulation', 0, 60),
        ('Controller Visualizer', '/simulation/visualizer', 'Visualize controller state', 'Simulation', 0, 61);
    
    PRINT 'Inserted default pages';
END
ELSE
BEGIN
    PRINT 'Default pages already exist';
END
GO

-- Verify
SELECT 'AppPages' as TableName, COUNT(*) as RowCount FROM AppPages
UNION ALL
SELECT 'UserPagePermissions', COUNT(*) FROM UserPagePermissions;
GO

PRINT '';
PRINT '==============================================';
PRINT 'Page permissions migration completed successfully!';
PRINT 'You can now restart your application and use the Page Permissions feature!';
PRINT '==============================================';
