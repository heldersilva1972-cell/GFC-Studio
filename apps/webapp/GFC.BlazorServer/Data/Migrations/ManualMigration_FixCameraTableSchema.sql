-- Comprehensive migration and repair script for the Cameras table
-- This script ensures all columns exist and repairs NULL values that cause "Data is Null" crashes.

USE [ClubMembership];
GO

-- 1. Ensure the Cameras table itself exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Cameras](
        [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] [nvarchar](100) NOT NULL DEFAULT 'New Camera',
        [RtspUrl] [nvarchar](500) NOT NULL DEFAULT 'rtsp://',
        [IsEnabled] [bit] NOT NULL DEFAULT 1,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE()
    );
    PRINT 'Created Cameras table from scratch.';
END
GO

-- 2. Add any columns that might be missing from an older schema
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'IpAddress')
    ALTER TABLE [dbo].[Cameras] ADD [IpAddress] NVARCHAR(45) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'Manufacturer')
    ALTER TABLE [dbo].[Cameras] ADD [Manufacturer] NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'Username')
    ALTER TABLE [dbo].[Cameras] ADD [Username] NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'Password')
    ALTER TABLE [dbo].[Cameras] ADD [Password] NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'IsEnabled')
    ALTER TABLE [dbo].[Cameras] ADD [IsEnabled] [bit] NOT NULL DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'CreatedAt')
    ALTER TABLE [dbo].[Cameras] ADD [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE();

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'UpdatedAt')
    ALTER TABLE [dbo].[Cameras] ADD [UpdatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE();
GO

-- 3. Data Repair: Fix NULL values in Required columns that cause runtime crashes
UPDATE [dbo].[Cameras] SET [Name] = 'Unnamed Camera' WHERE [Name] IS NULL;
UPDATE [dbo].[Cameras] SET [RtspUrl] = 'rtsp://' WHERE [RtspUrl] IS NULL;
UPDATE [dbo].[Cameras] SET [IsEnabled] = 1 WHERE [IsEnabled] IS NULL;
UPDATE [dbo].[Cameras] SET [CreatedAt] = GETUTCDATE() WHERE [CreatedAt] IS NULL;
UPDATE [dbo].[Cameras] SET [UpdatedAt] = GETUTCDATE() WHERE [UpdatedAt] IS NULL;
GO

PRINT 'Cameras table repair and health check completed successfully.';
