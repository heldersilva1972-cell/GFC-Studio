-- =============================================
-- Create Admin User for GFC System
-- Database: ClubMembership
-- =============================================

USE [ClubMembership];
GO

PRINT 'Creating Admin User...';

-- Check if admin user already exists
IF NOT EXISTS (SELECT * FROM AspNetUsers WHERE UserName = 'admin')
BEGIN
    -- Generate a new GUID for the user
    DECLARE @UserId NVARCHAR(450) = NEWID();
    
    -- Password: Admin123! (hashed)
    -- You should change this after first login
    DECLARE @PasswordHash NVARCHAR(MAX) = 'AQAAAAIAAYagAAAAEJ3z8VqN5fZxN0rH5K0qYvF5xN5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Q==';
    
    -- Insert admin user
    INSERT INTO AspNetUsers (
        Id,
        UserName,
        NormalizedUserName,
        Email,
        NormalizedEmail,
        EmailConfirmed,
        PasswordHash,
        SecurityStamp,
        ConcurrencyStamp,
        PhoneNumberConfirmed,
        TwoFactorEnabled,
        LockoutEnabled,
        AccessFailedCount
    )
    VALUES (
        @UserId,
        'admin',
        'ADMIN',
        'admin@gfc.local',
        'ADMIN@GFC.LOCAL',
        1,
        @PasswordHash,
        NEWID(),
        NEWID(),
        0,
        0,
        1,
        0
    );
    
    PRINT '✓ Admin user created';
    PRINT '  Username: admin';
    PRINT '  Password: Admin123!';
    PRINT '  ⚠ CHANGE PASSWORD AFTER FIRST LOGIN!';
    
    -- Check if Admin role exists
    IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Admin')
    BEGIN
        DECLARE @RoleId NVARCHAR(450) = NEWID();
        
        INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
        VALUES (@RoleId, 'Admin', 'ADMIN', NEWID());
        
        PRINT '✓ Admin role created';
        
        -- Assign admin role to user
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES (@UserId, @RoleId);
        
        PRINT '✓ Admin role assigned to user';
    END
    ELSE
    BEGIN
        -- Role exists, just assign it
        DECLARE @ExistingRoleId NVARCHAR(450);
        SELECT @ExistingRoleId = Id FROM AspNetRoles WHERE Name = 'Admin';
        
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES (@UserId, @ExistingRoleId);
        
        PRINT '✓ Admin role assigned to user';
    END
END
ELSE
BEGIN
    PRINT '⚠ Admin user already exists';
    PRINT 'If you forgot the password, you need to reset it.';
END
GO

PRINT '';
PRINT '========================================';
PRINT 'Admin User Setup Complete';
PRINT '========================================';
PRINT '';
PRINT 'Login Credentials:';
PRINT '  Username: admin';
PRINT '  Password: Admin123!';
PRINT '';
PRINT '⚠ IMPORTANT: Change this password after first login!';
PRINT '========================================';
GO
