USE master;
GO
DECLARE @dbName NVARCHAR(128) = 'ClubMembership';
DECLARE @bakFile NVARCHAR(MAX) = 'C:\Users\Public\ClubMembership_Restore.bak';

PRINT 'Dropping existing database if it exists...';
IF EXISTS (SELECT name FROM sys.databases WHERE name = @dbName)
BEGIN
    ALTER DATABASE [ClubMembership] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [ClubMembership];
END

PRINT 'Starting Restore from Public folder...';

-- Restore with moving files to the workspace directory
RESTORE DATABASE [ClubMembership] 
FROM DISK = @bakFile
WITH REPLACE,
MOVE 'ClubMembership' TO 'C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\ClubMembership.mdf',
MOVE 'ClubMembership_log' TO 'C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\ClubMembership_log.ldf';

PRINT 'Restore Complete.';
GO
