-- Diagnostic script to check user permissions for mobile pages
-- This helps troubleshoot why mobile pages aren't showing for users

-- 1. Check if mobile pages exist in AppPages table
SELECT 
    PageId,
    PageName,
    PageRoute,
    IsActive
FROM AppPages
WHERE PageRoute LIKE '/mobile%'
ORDER BY PageRoute;

-- 2. Check a specific user's permissions
DECLARE @UserId INT = 2; -- Change this to the user you're testing

SELECT 
    u.UserId,
    u.Username,
    u.IsAdmin,
    p.PageId,
    p.PageName,
    p.PageRoute
FROM AppUsers u
LEFT JOIN UserPagePermissions upp ON u.UserId = upp.UserId
LEFT JOIN AppPages p ON upp.PageId = p.PageId
WHERE u.UserId = @UserId
ORDER BY p.PageRoute;

-- 3. Check if the user has ANY permissions at all
SELECT 
    u.UserId,
    u.Username,
    COUNT(upp.PageId) AS TotalPermissions
FROM AppUsers u
LEFT JOIN UserPagePermissions upp ON u.UserId = upp.UserId
WHERE u.UserId = @UserId
GROUP BY u.UserId, u.Username;

-- 4. Grant mobile permissions to a user (UNCOMMENT TO USE)
-- DECLARE @TargetUserId INT = 2; -- Change this
-- 
-- -- Get mobile page IDs
-- DECLARE @ShiftReportPageId INT = (SELECT PageId FROM AppPages WHERE PageRoute = '/mobile/shift-report');
-- DECLARE @AnalyticsPageId INT = (SELECT PageId FROM AppPages WHERE PageRoute = '/mobile/analytics');
-- DECLARE @MobileHubPageId INT = (SELECT PageId FROM AppPages WHERE PageRoute = '/mobile');
-- 
-- -- Insert permissions if they don't exist
-- IF @ShiftReportPageId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM UserPagePermissions WHERE UserId = @TargetUserId AND PageId = @ShiftReportPageId)
-- BEGIN
--     INSERT INTO UserPagePermissions (UserId, PageId) VALUES (@TargetUserId, @ShiftReportPageId);
--     PRINT 'Granted Shift Report permission';
-- END
-- 
-- IF @AnalyticsPageId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM UserPagePermissions WHERE UserId = @TargetUserId AND PageId = @AnalyticsPageId)
-- BEGIN
--     INSERT INTO UserPagePermissions (UserId, PageId) VALUES (@TargetUserId, @AnalyticsPageId);
--     PRINT 'Granted Analytics permission';
-- END
-- 
-- IF @MobileHubPageId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM UserPagePermissions WHERE UserId = @TargetUserId AND PageId = @MobileHubPageId)
-- BEGIN
--     INSERT INTO UserPagePermissions (UserId, PageId) VALUES (@TargetUserId, @MobileHubPageId);
--     PRINT 'Granted Mobile Hub permission';
-- END
