-- =================================================
-- MANUAL USER SEEDING (FORCED UPDATE)
-- =================================================

USE [ClubMembership];
GO

PRINT 'Setting up admin user password...';

-- Password is: Admin123!
-- SHA256(Admin123!) = eJIaLDaCl5IDkjkQwmiA6oDBC3GUzDhnD15xRjP4bjo=
DECLARE @PasswordHash NVARCHAR(MAX) = 'eJIaLDaCl5IDkjkQwmiA6oDBC3GUzDhnD15xRjP4bjo=';

IF EXISTS (SELECT 1 FROM AppUsers WHERE Username = 'admin')
BEGIN
    PRINT 'User "admin" exists. Updating password...';
    UPDATE AppUsers 
    SET PasswordHash = @PasswordHash,
        IsAdmin = 1,
        IsActive = 1,
        PasswordChangeRequired = 0
    WHERE Username = 'admin';
END
ELSE
BEGIN
    PRINT 'User "admin" does not exist. Creating...';
    INSERT INTO [dbo].[AppUsers] (
        Username, 
        Email, 
        PasswordHash, 
        IsAdmin, 
        IsActive, 
        CreatedDate, 
        CreatedBy, 
        PasswordChangeRequired
    )
    VALUES (
        'admin', 
        'admin@gfc.local', 
        @PasswordHash, 
        1, 
        1, 
        GETUTCDATE(), 
        'ManualSeed', 
        0
    );
END
GO

PRINT 'Verification:';
SELECT UserId, Username, PasswordHash, IsAdmin, IsActive FROM AppUsers WHERE Username = 'admin';
GO
