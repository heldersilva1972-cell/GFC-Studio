-- Manual migration to add Username and Password columns to Cameras table
-- Run this script manually in SQL Server Management Studio or via sqlcmd

USE [GFC];
GO

-- Add Username column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'Username')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [Username] NVARCHAR(100) NULL;
    PRINT 'Added Username column to Cameras table';
END
ELSE
BEGIN
    PRINT 'Username column already exists in Cameras table';
END
GO

-- Add Password column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'Password')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [Password] NVARCHAR(100) NULL;
    PRINT 'Added Password column to Cameras table';
END
ELSE
BEGIN
    PRINT 'Password column already exists in Cameras table';
END
GO

PRINT 'Migration completed successfully';
