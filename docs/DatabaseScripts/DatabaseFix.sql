-- =============================================
-- COMPLETE FIX - Rental Management Database
-- Created: 2025-12-25
-- Description: Ensures all required columns and tables exist
-- =============================================

PRINT '-----------------------------------------';
PRINT 'Starting Rental Management Database Fix';
PRINT '-----------------------------------------';

-- 1. Create/Update HallRentalRequests Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HallRentalRequests')
BEGIN
    CREATE TABLE [dbo].[HallRentalRequests] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ApplicantName] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [RequesterName] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [RequesterEmail] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [RequesterPhone] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [EventDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [MemberStatus] BIT NOT NULL DEFAULT 0,
        [GuestCount] INT NOT NULL DEFAULT 0,
        [RulesAgreed] BIT NOT NULL DEFAULT 0,
        [KitchenUsage] BIT NOT NULL DEFAULT 0,
        [RequestedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [TotalPrice] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [Status] NVARCHAR(MAX) NOT NULL DEFAULT 'Pending',
        [ApprovedBy] NVARCHAR(MAX) NULL,
        [ApprovalDate] DATETIME2 NULL,
        [InternalNotes] NVARCHAR(MAX) NULL
    );
    PRINT '✓ Created HallRentalRequests table';
END
ELSE
BEGIN
    PRINT 'HallRentalRequests table already exists - checking for missing columns...';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'ApplicantName')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [ApplicantName] NVARCHAR(MAX) NOT NULL DEFAULT '';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'EventDate')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [EventDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'TotalPrice')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [TotalPrice] DECIMAL(18,2) NOT NULL DEFAULT 0;
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'InternalNotes')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [InternalNotes] NVARCHAR(MAX) NULL;
        
    PRINT '✓ Verified/Updated HallRentalRequests columns';
END
GO

-- 2. Create/Update AvailabilityCalendars Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AvailabilityCalendars')
BEGIN
    CREATE TABLE [dbo].[AvailabilityCalendars] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Date] DATETIME2 NOT NULL,
        [Status] NVARCHAR(MAX) NOT NULL -- Available, Booked, Pending
    );
    PRINT '✓ Created AvailabilityCalendars table';
END
ELSE
BEGIN
    PRINT '✓ AvailabilityCalendars table already exists';
END
GO

PRINT '-----------------------------------------';
PRINT 'Database fix completed successfully!';
PRINT '-----------------------------------------';
GO
