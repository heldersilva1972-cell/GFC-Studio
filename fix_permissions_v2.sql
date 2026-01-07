USE master;
GO

-- 1. Grant Access to ALL IIS Application Pools (The safe, catch-all fix)
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'BUILTIN\IIS_IUSRS')
BEGIN
    CREATE LOGIN [BUILTIN\IIS_IUSRS] FROM WINDOWS;
END
GO

USE ClubMembership;
GO

-- 2. Add the IIS Group to the Database
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'BUILTIN\IIS_IUSRS')
BEGIN
    CREATE USER [BUILTIN\IIS_IUSRS] FOR LOGIN [BUILTIN\IIS_IUSRS];
END
ALTER ROLE db_owner ADD MEMBER [BUILTIN\IIS_IUSRS];
GO

-- 3. Fix Your User Access (Helder Silva)
-- If you are the creator, you are likely mapped to 'dbo'.
-- We will just ensure 'dbo' is owned by 'sa' to avoid ownership conflicts,
-- and then try to add you as a user if you aren't already mapped.
ALTER AUTHORIZATION ON DATABASE::ClubMembership TO sa;
GO

DECLARE @CurrentLogin NVARCHAR(128) = SYSTEM_USER;
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @CurrentLogin)
BEGIN
    -- Only try to add if the user isn't already dbo or aliased
    BEGIN TRY
        EXEC('CREATE USER [' + @CurrentLogin + '] FOR LOGIN [' + @CurrentLogin + ']');
        EXEC('ALTER ROLE db_owner ADD MEMBER [' + @CurrentLogin + ']');
    END TRY
    BEGIN CATCH
        PRINT 'User likely already exists or is dbo. Skipping.';
    END CATCH
END
ELSE
BEGIN
     EXEC('ALTER ROLE db_owner ADD MEMBER [' + @CurrentLogin + ']');
END
GO
