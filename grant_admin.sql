USE master;
GO
-- Ensure the login exists
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'DESKTOP-D85HJ4T\Helder Silva')
BEGIN
    CREATE LOGIN [DESKTOP-D85HJ4T\Helder Silva] FROM WINDOWS;
END
GO
-- Grant sysadmin to be sure
ALTER SERVER ROLE sysadmin ADD MEMBER [DESKTOP-D85HJ4T\Helder Silva];
GO
-- Ensure the user is mapped in the database just in case
USE ClubMembership;
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DESKTOP-D85HJ4T\Helder Silva')
BEGIN
    CREATE USER [DESKTOP-D85HJ4T\Helder Silva] FOR LOGIN [DESKTOP-D85HJ4T\Helder Silva];
END
GO
ALTER ROLE db_owner ADD MEMBER [DESKTOP-D85HJ4T\Helder Silva];
GO
