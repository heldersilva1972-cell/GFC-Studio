-- Quick fix: Add event detail columns to HallRentalRequests
-- Run this in SQL Server Management Studio or your database tool

USE ClubMembership;
GO

-- Add EventType column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'EventType')
BEGIN
    ALTER TABLE [dbo].[HallRentalRequests] ADD [EventType] NVARCHAR(100) NULL;
    PRINT 'Added EventType column';
END
ELSE
BEGIN
    PRINT 'EventType column already exists';
END

-- Add StartTime column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'StartTime')
BEGIN
    ALTER TABLE [dbo].[HallRentalRequests] ADD [StartTime] NVARCHAR(50) NULL;
    PRINT 'Added StartTime column';
END
ELSE
BEGIN
    PRINT 'StartTime column already exists';
END

-- Add EndTime column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'EndTime')
BEGIN
    ALTER TABLE [dbo].[HallRentalRequests] ADD [EndTime] NVARCHAR(50) NULL;
    PRINT 'Added EndTime column';
END
ELSE
BEGIN
    PRINT 'EndTime column already exists';
END

-- Add payment columns to WebsiteSettings
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'EnableOnlineRentalsPayment')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [EnableOnlineRentalsPayment] BIT NOT NULL DEFAULT 0;
    PRINT 'Added EnableOnlineRentalsPayment column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PaymentGatewayUrl')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [PaymentGatewayUrl] NVARCHAR(500) NULL;
    PRINT 'Added PaymentGatewayUrl column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PaymentGatewayApiKey')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [PaymentGatewayApiKey] NVARCHAR(500) NULL;
    PRINT 'Added PaymentGatewayApiKey column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'MaxHallRentalDurationHours')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings] ADD [MaxHallRentalDurationHours] INT NULL DEFAULT 8;
    PRINT 'Added MaxHallRentalDurationHours column';
END

PRINT 'Database update complete!';
GO
