USE master;
GO

-- 1. Ensure the Login exists
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'gfc_app_user')
BEGIN
    CREATE LOGIN gfc_app_user WITH PASSWORD = 'StrongPassword123!', CHECK_POLICY = OFF;
END
ELSE
BEGIN
    -- Reset password just in case restoring the DB confused it
    ALTER LOGIN gfc_app_user WITH PASSWORD = 'StrongPassword123!';
END
GO

USE ClubMembership;
GO

-- 2. Fix the "Orphaned User" problem
-- (This happens when you restore a DB: the 'gfc_app_user' inside the DB loses its link to the Server Login)
DECLARE @SqlUser NVARCHAR(50) = 'gfc_app_user';

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = @SqlUser)
BEGIN
    -- Relink the DB user to the Server login
    ALTER USER gfc_app_user WITH LOGIN = gfc_app_user;
    PRINT 'Relinked orphaned user.';
END
ELSE
BEGIN
    -- Create if it was missing from the backup
    CREATE USER gfc_app_user FOR LOGIN gfc_app_user;
    PRINT 'Created new user in restored DB.';
END

-- 3. Grant Permissions
ALTER ROLE db_owner ADD MEMBER gfc_app_user;
PRINT 'Granted db_owner permission.';
GO
