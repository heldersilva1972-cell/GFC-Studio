-- Migration: Register Administrative Logs page
-- Date: 2026-03-08

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/admin/audit-logs')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Administrative Logs', '/admin/audit-logs', 'Management and security audit trails', 'ADMINISTRATION', 1, 1, 15);
    
    PRINT 'Registered Administrative Logs page.';
END
ELSE
BEGIN
    UPDATE AppPages 
    SET Category = 'ADMINISTRATION', 
        PageName = 'Administrative Logs',
        IsActive = 1
    WHERE PageRoute = '/admin/audit-logs';
    
    PRINT 'Updated Administrative Logs page.';
END

-- Ensure Infrastructure Hub is in the correct category for permissions
UPDATE AppPages 
SET Category = 'SYSTEM', 
    PageName = 'Infrastructure Hub'
WHERE PageRoute = '/admin/operations';

-- Grant to all current admins automatically
INSERT INTO UserPagePermissions (UserId, PageId, CanAccess, GrantedDate, GrantedBy, ReceivePush)
SELECT u.UserId, p.PageId, 1, GETDATE(), 'System', 0
FROM AppUsers u
CROSS JOIN AppPages p
WHERE u.IsAdmin = 1 AND p.PageRoute = '/admin/audit-logs'
AND NOT EXISTS (
    SELECT 1 FROM UserPagePermissions 
    WHERE UserId = u.UserId AND PageId = p.PageId
);
GO
