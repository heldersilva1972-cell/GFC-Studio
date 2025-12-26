-- =============================================
-- FIX RENTAL MANAGEMENT DATABASE
-- Created: 2025-12-25
-- Description: Ensures all rental-related tables and columns exist and match C# models
-- USE ClubMembership; -- Uncomment and change if using a different DB
-- GO
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
        [EventDate] DATETIME2 NOT NULL,
        [MemberStatus] BIT NOT NULL DEFAULT 0,
        [GuestCount] INT NOT NULL DEFAULT 0,
        [RulesAgreed] BIT NOT NULL DEFAULT 0,
        [KitchenUsage] BIT NOT NULL DEFAULT 0,
        [RequestedDate] DATETIME2 NOT NULL,
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
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequesterName')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequesterName] NVARCHAR(MAX) NOT NULL DEFAULT '';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequesterEmail')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequesterEmail] NVARCHAR(MAX) NOT NULL DEFAULT '';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequesterPhone')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequesterPhone] NVARCHAR(MAX) NOT NULL DEFAULT '';
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'EventDate')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [EventDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE();

    -- Missing boolean/int fields fix
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'MemberStatus')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [MemberStatus] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'GuestCount')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [GuestCount] INT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RulesAgreed')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [RulesAgreed] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'KitchenUsage')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [KitchenUsage] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequestedDate')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequestedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE();

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'Status')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [Status] NVARCHAR(MAX) NOT NULL DEFAULT 'Pending';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'ApprovedBy')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [ApprovedBy] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'ApprovalDate')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [ApprovalDate] DATETIME2 NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'DeniedBy')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [DeniedBy] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'DenialDate')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [DenialDate] DATETIME2 NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'StatusChangedBy')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [StatusChangedBy] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'StatusChangedDate')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [StatusChangedDate] DATETIME2 NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'CreatedDate')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE();

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'EventType')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [EventType] NVARCHAR(100) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'StartTime')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [StartTime] NVARCHAR(50) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'EndTime')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [EndTime] NVARCHAR(50) NULL;
        
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

-- 3. Create/Update HallRentals Table (Just in case)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HallRentals')
BEGIN
    CREATE TABLE [dbo].[HallRentals] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ApplicantName] NVARCHAR(MAX) NOT NULL,
        [ContactInfo] NVARCHAR(MAX) NOT NULL,
        [EventDate] DATETIME2 NOT NULL,
        [Status] NVARCHAR(MAX) NOT NULL,
        [GuestCount] INT NOT NULL,
        [KitchenUsed] BIT NOT NULL DEFAULT 0,
        [TotalPrice] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [InternalNotes] NVARCHAR(MAX) NULL
    );
    PRINT '✓ Created HallRentals table';
END
ELSE
BEGIN
    PRINT '✓ HallRentals table already exists';
END
GO

PRINT '-----------------------------------------';
PRINT 'Database fix completed successfully!';
PRINT '-----------------------------------------';
GO
