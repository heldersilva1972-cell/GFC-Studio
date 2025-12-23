-- Comprehensive migration to ensure ALL required columns exist in the Cameras table
-- This script safely checks and adds Name, RtspUrl, IpAddress, Manufacturer, Username, Password, IsEnabled, CreatedAt, and UpdatedAt.

USE [ClubMembership];
GO

-- 1. Ensure Table Exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Cameras](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [RtspUrl] [nvarchar](500) NOT NULL,
        [IsEnabled] [bit] NOT NULL DEFAULT 1,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Cameras] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT 'Created Cameras table';
END
GO

-- 2. Check and Add Missing Columns
-- IpAddress
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'IpAddress')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [IpAddress] NVARCHAR(45) NULL;
    PRINT 'Added IpAddress column';
END

-- Manufacturer
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'Manufacturer')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [Manufacturer] NVARCHAR(100) NULL;
    PRINT 'Added Manufacturer column';
END

-- Username
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'Username')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [Username] NVARCHAR(100) NULL;
    PRINT 'Added Username column';
END

-- Password
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'Password')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [Password] NVARCHAR(100) NULL;
    PRINT 'Added Password column';
END

-- IsEnabled (Double check)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'IsEnabled')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [IsEnabled] [bit] NOT NULL DEFAULT 1;
    PRINT 'Added IsEnabled column';
END

-- CreatedAt (Double check)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'CreatedAt')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE();
    PRINT 'Added CreatedAt column';
END

-- UpdatedAt (Double check)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND name = 'UpdatedAt')
BEGIN
    ALTER TABLE [dbo].[Cameras] ADD [UpdatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE();
    PRINT 'Added UpdatedAt column';
END
GO

PRINT 'Cameras table health check and migration completed successfully';
