USE master;
GO
-- Create SQL Login for the Web App (Bypasses Windows Auth issues)
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'gfc_app_user')
BEGIN
    CREATE LOGIN gfc_app_user WITH PASSWORD = 'StrongPassword123!', CHECK_POLICY = OFF;
    PRINT 'Created SQL Login: gfc_app_user';
END
GO

USE ClubMembership;
GO
-- Create User in Database
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'gfc_app_user')
BEGIN
    CREATE USER gfc_app_user FOR LOGIN gfc_app_user;
    PRINT 'Created DB User: gfc_app_user';
END
ALTER ROLE db_owner ADD MEMBER gfc_app_user;
PRINT 'Granted db_owner to gfc_app_user';
GO
