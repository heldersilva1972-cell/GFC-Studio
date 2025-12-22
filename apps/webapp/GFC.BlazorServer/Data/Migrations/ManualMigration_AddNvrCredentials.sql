-- Add NVR Credentials columns to SystemSettings table
-- Run this script manually in SQL Server Management Studio

USE ClubMembership;
GO

-- Add NVR credentials columns if they don't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrIpAddress')
BEGIN
    ALTER TABLE [dbo].[SystemSettings]
    ADD [NvrIpAddress] NVARCHAR(50) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrPort')
BEGIN
    ALTER TABLE [dbo].[SystemSettings]
    ADD [NvrPort] INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrUsername')
BEGIN
    ALTER TABLE [dbo].[SystemSettings]
    ADD [NvrUsername] NVARCHAR(100) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrPassword')
BEGIN
    ALTER TABLE [dbo].[SystemSettings]
    ADD [NvrPassword] NVARCHAR(255) NULL;
END
GO

PRINT 'NVR credentials columns added successfully!';
