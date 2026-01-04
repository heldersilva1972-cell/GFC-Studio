-- =============================================
-- Verify Database Setup
-- Check if admin user was created correctly
-- =============================================

USE [ClubMembership];
GO

PRINT '========================================';
PRINT 'Database Verification';
PRINT '========================================';
PRINT '';

-- Check if AspNetUsers table exists and has data
PRINT 'Checking AspNetUsers table...';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUsers')
BEGIN
    DECLARE @UserCount INT;
    SELECT @UserCount = COUNT(*) FROM AspNetUsers;
    PRINT '✓ AspNetUsers table exists';
    PRINT '  Total users: ' + CAST(@UserCount AS NVARCHAR(10));
    
    -- Show all users
    SELECT 
        UserName,
        Email,
        EmailConfirmed,
        LockoutEnabled,
        AccessFailedCount,
        CASE WHEN PasswordHash IS NULL THEN 'NO PASSWORD' ELSE 'HAS PASSWORD' END AS PasswordStatus
    FROM AspNetUsers;
END
ELSE
BEGIN
    PRINT '✗ AspNetUsers table does NOT exist!';
END
GO

-- Check if AspNetRoles table exists and has data
PRINT '';
PRINT 'Checking AspNetRoles table...';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetRoles')
BEGIN
    DECLARE @RoleCount INT;
    SELECT @RoleCount = COUNT(*) FROM AspNetRoles;
    PRINT '✓ AspNetRoles table exists';
    PRINT '  Total roles: ' + CAST(@RoleCount AS NVARCHAR(10));
    
    -- Show all roles
    SELECT Name FROM AspNetRoles;
END
ELSE
BEGIN
    PRINT '✗ AspNetRoles table does NOT exist!';
END
GO

-- Check user-role assignments
PRINT '';
PRINT 'Checking User-Role assignments...';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserRoles')
BEGIN
    SELECT 
        u.UserName,
        r.Name AS RoleName
    FROM AspNetUserRoles ur
    JOIN AspNetUsers u ON ur.UserId = u.Id
    JOIN AspNetRoles r ON ur.RoleId = r.Id;
END
GO

PRINT '';
PRINT '========================================';
PRINT 'Verification Complete';
PRINT '========================================';
GO
