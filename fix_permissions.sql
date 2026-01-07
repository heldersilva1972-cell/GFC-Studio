USE [master]
GO
-- Create the login for the Windows user if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'DESKTOP-D85HJ4T\Helder Silva')
BEGIN
    CREATE LOGIN [DESKTOP-D85HJ4T\Helder Silva] FROM WINDOWS WITH DEFAULT_DATABASE=[master]
END
GO

USE [ClubMembership]
GO
-- Create the user in the database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DESKTOP-D85HJ4T\Helder Silva')
BEGIN
    CREATE USER [DESKTOP-D85HJ4T\Helder Silva] FOR LOGIN [DESKTOP-D85HJ4T\Helder Silva]
END
GO
-- Grant permissions
ALTER ROLE [db_owner] ADD MEMBER [DESKTOP-D85HJ4T\Helder Silva]
GO
