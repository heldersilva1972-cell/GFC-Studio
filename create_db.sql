USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ClubMembership')
BEGIN
    CREATE DATABASE ClubMembership;
    PRINT '>>> Created ClubMembership Database';
END
GO
