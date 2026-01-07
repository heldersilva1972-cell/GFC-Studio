USE master;
GO
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'IIS AppPool\GFCWebApp')
BEGIN
    CREATE LOGIN [IIS AppPool\GFCWebApp] FROM WINDOWS;
END
GO

USE ClubMembership;
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'IIS AppPool\GFCWebApp')
BEGIN
    CREATE USER [IIS AppPool\GFCWebApp] FOR LOGIN [IIS AppPool\GFCWebApp];
END
ALTER ROLE db_owner ADD MEMBER [IIS AppPool\GFCWebApp];
GO
