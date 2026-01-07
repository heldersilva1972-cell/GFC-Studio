USE master;
GO

-- 1. Close Existing Connections to allow restore
ALTER DATABASE ClubMembership SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- 2. Restore the Database
RESTORE DATABASE ClubMembership 
FROM DISK = 'C:\Users\Public\Documents\ClubMembership_Migration.bak'
WITH REPLACE, RECOVERY;
GO

-- 3. Reset to Multi-User
ALTER DATABASE ClubMembership SET MULTI_USER;
GO

-- 4. Fix User Mappings (Orphaned Users)
USE ClubMembership;
GO
-- Re-map Helder (Windows)
IF EXISTS (SELECT * FROM sys.server_principals WHERE name = 'DESKTOP-D85HJ4T\Helder Silva')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'DESKTOP-D85HJ4T\Helder Silva')
    BEGIN
        CREATE USER [DESKTOP-D85HJ4T\Helder Silva] FOR LOGIN [DESKTOP-D85HJ4T\Helder Silva];
    END
    ALTER ROLE db_owner ADD MEMBER [DESKTOP-D85HJ4T\Helder Silva];
END

-- Re-map IIS Users (Windows)
IF EXISTS (SELECT * FROM sys.server_principals WHERE name = 'BUILTIN\IIS_IUSRS')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'BUILTIN\IIS_IUSRS')
    BEGIN
        CREATE USER [BUILTIN\IIS_IUSRS] FOR LOGIN [BUILTIN\IIS_IUSRS];
    END
    ALTER ROLE db_owner ADD MEMBER [BUILTIN\IIS_IUSRS];
END

-- Re-map SQL User (Web App)
IF EXISTS (SELECT * FROM sys.server_principals WHERE name = 'gfc_app_user')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'gfc_app_user')
    BEGIN
        CREATE USER gfc_app_user FOR LOGIN gfc_app_user;
    END
    ELSE
    BEGIN
        -- Fix orphaned user if it exists in DB but lost link to Login
        ALTER USER gfc_app_user WITH LOGIN = gfc_app_user;
    END
    ALTER ROLE db_owner ADD MEMBER gfc_app_user;
END
GO
