-- Fixed Migration Script for ClubMembership Database
-- Run this in SSMS or Azure Data Studio

USE ClubMembership;
GO

-- Create UserPagePermissions table (AppPages already exists)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'UserPagePermissions')
BEGIN
    DROP TABLE UserPagePermissions;
    PRINT 'Dropped existing UserPagePermissions table';
END
GO

CREATE TABLE UserPagePermissions (
    PermissionId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    PageId INT NOT NULL,
    CanAccess BIT NOT NULL DEFAULT 1,
    GrantedDate DATETIME NOT NULL DEFAULT GETDATE(),
    GrantedBy NVARCHAR(100) NULL,
    
    CONSTRAINT FK_UserPagePermissions_AppUsers FOREIGN KEY (UserId) 
        REFERENCES AppUsers(UserId) ON DELETE CASCADE,
    CONSTRAINT FK_UserPagePermissions_AppPages FOREIGN KEY (PageId) 
        REFERENCES AppPages(PageId) ON DELETE CASCADE,
    CONSTRAINT UQ_UserPagePermissions UNIQUE (UserId, PageId)
);

CREATE INDEX IX_UserPagePermissions_UserId ON UserPagePermissions(UserId);
CREATE INDEX IX_UserPagePermissions_PageId ON UserPagePermissions(PageId);

PRINT 'Created UserPagePermissions table with correct foreign key to AppUsers';
GO

-- Verify
SELECT 'AppPages' as TableName, COUNT(*) as [RowCount] FROM AppPages
UNION ALL
SELECT 'UserPagePermissions', COUNT(*) FROM UserPagePermissions;
GO

PRINT '';
PRINT '==============================================';
PRINT 'Page permissions migration completed successfully!';
PRINT 'Tables created:';
PRINT '  - AppPages (33 pages)';
PRINT '  - UserPagePermissions (ready for use)';
PRINT '';
PRINT 'You can now restart your application and use the Page Permissions feature!';
PRINT '==============================================';
