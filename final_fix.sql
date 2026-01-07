USE [master];
GO
-- 1. Ensure the login has the "Super User" (sysadmin) role
-- This is the most powerful permission and overrides everything else.
ALTER SERVER ROLE [sysadmin] ADD MEMBER [DESKTOP-D85HJ4T\Helder Silva];
GO

USE [ClubMembership];
GO
-- 2. Fix the specific database user
-- If the user exists but is broken (orphaned), we drop it.
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DESKTOP-D85HJ4T\Helder Silva')
BEGIN
    DROP USER [DESKTOP-D85HJ4T\Helder Silva];
END
GO
-- Re-create the user freshly linked to your login
CREATE USER [DESKTOP-D85HJ4T\Helder Silva] FOR LOGIN [DESKTOP-D85HJ4T\Helder Silva];
GO
-- Make sure you own the database
ALTER ROLE [db_owner] ADD MEMBER [DESKTOP-D85HJ4T\Helder Silva];
GO
