USE master;
GO

-- Forcefully create database if missing
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ClubMembership')
BEGIN
    CREATE DATABASE ClubMembership;
    PRINT 'Created ClubMembership database.';
END
ELSE
BEGIN
    PRINT 'ClubMembership database already exists.';
END
GO

-- Create Login for Helder Silva (Hardcoded)
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'DESKTOP-D85HJ4T\Helder Silva')
BEGIN
    CREATE LOGIN [DESKTOP-D85HJ4T\Helder Silva] FROM WINDOWS;
    PRINT 'Created Login for Helder Silva.';
END
ELSE
BEGIN
    PRINT 'Login for Helder Silva already exists.';
END
GO

USE ClubMembership;
GO

-- Drop User if exists to reset mapping (optional, but effectively clears bad states)
-- IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'DESKTOP-D85HJ4T\Helder Silva')
-- BEGIN
--    DROP USER [DESKTOP-D85HJ4T\Helder Silva];
-- END

-- Create User in Database
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'DESKTOP-D85HJ4T\Helder Silva')
BEGIN
    CREATE USER [DESKTOP-D85HJ4T\Helder Silva] FOR LOGIN [DESKTOP-D85HJ4T\Helder Silva];
    PRINT 'Created User for Helder Silva in ClubMembership.';
END
ELSE
BEGIN
    PRINT 'User for Helder Silva already exists in ClubMembership.';
END
GO

-- Grant dbo
ALTER ROLE db_owner ADD MEMBER [DESKTOP-D85HJ4T\Helder Silva];
PRINT 'Granted db_owner to Helder Silva.';
GO

-- Grant IIS_IUSRS (Web Server)
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'BUILTIN\IIS_IUSRS')
BEGIN
    CREATE USER [BUILTIN\IIS_IUSRS] FOR LOGIN [BUILTIN\IIS_IUSRS];
END
ALTER ROLE db_owner ADD MEMBER [BUILTIN\IIS_IUSRS];
PRINT 'Granted db_owner to IIS_IUSRS.';
GO
