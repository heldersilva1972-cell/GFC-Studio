-- Manual migration to add IpAddress and Manufacturer columns to Cameras table
-- Run this script manually in SQL Server Management Studio or via sqlcmd

USE [ClubMembership];
GO

-- Add IpAddress column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'IpAddress')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [IpAddress] NVARCHAR(45) NULL;
    PRINT 'Added IpAddress column to Cameras table';
END
ELSE
BEGIN
    PRINT 'IpAddress column already exists in Cameras table';
END
GO

-- Add Manufacturer column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'Manufacturer')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [Manufacturer] NVARCHAR(100) NULL;
    PRINT 'Added Manufacturer column to Cameras table';
END
ELSE
BEGIN
    PRINT 'Manufacturer column already exists in Cameras table';
END
GO

PRINT 'Migration completed successfully';
