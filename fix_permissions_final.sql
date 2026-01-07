USE master;
GO
-- 1. Ensure the Database Exists
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ClubMembership')
BEGIN
    CREATE DATABASE ClubMembership;
    PRINT '>>> Created new database ClubMembership on SQLEXPRESS';
END
GO

-- 2. Ensure the Login Exists for your User
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'DESKTOP-D85HJ4T\Helder Silva')
BEGIN
    CREATE LOGIN [DESKTOP-D85HJ4T\Helder Silva] FROM WINDOWS;
    PRINT '>>> Created Login for Helder Silva';
END
GO

-- 3. Ensure the Login Exists for the IIS AppPool (for the published site)
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'IIS AppPool\GFCWebApp')
BEGIN
    CREATE LOGIN [IIS AppPool\GFCWebApp] FROM WINDOWS;
    PRINT '>>> Created Login for IIS AppPool';
END
GO

USE ClubMembership;
GO

-- 4. Grant Permissions to Helder Silva
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'DESKTOP-D85HJ4T\Helder Silva')
BEGIN
    CREATE USER [DESKTOP-D85HJ4T\Helder Silva] FOR LOGIN [DESKTOP-D85HJ4T\Helder Silva];
    PRINT '>>> Created User for Helder Silva in Database';
END
ALTER ROLE db_owner ADD MEMBER [DESKTOP-D85HJ4T\Helder Silva];
PRINT '>>> Added Helder Silva to db_owner';

-- 5. Grant Permissions to IIS AppPool
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'IIS AppPool\GFCWebApp')
BEGIN
    CREATE USER [IIS AppPool\GFCWebApp] FOR LOGIN [IIS AppPool\GFCWebApp];
    PRINT '>>> Created User for IIS AppPool in Database';
END
ALTER ROLE db_owner ADD MEMBER [IIS AppPool\GFCWebApp];
PRINT '>>> Added IIS AppPool to db_owner';
GO
