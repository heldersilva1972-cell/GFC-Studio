-- =============================================
-- FIX RENTAL MANAGEMENT DATABASE
-- Created: 2025-12-25
-- Description: Ensures all rental-related tables and columns exist and match C# models
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
        [AvEquipmentUsage] BIT NOT NULL DEFAULT 0,
        [SecurityDepositPaid] BIT NOT NULL DEFAULT 0,
        [RequestedDate] DATETIME2 NOT NULL,
        [TotalPrice] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [IsPaid] BIT NOT NULL DEFAULT 0,
        [PaymentMethod] NVARCHAR(100) NULL,
        [PaymentDate] DATETIME2 NULL,
        [RenterType] NVARCHAR(50) NOT NULL DEFAULT 'Non-Member',
        [Status] NVARCHAR(MAX) NOT NULL DEFAULT 'Pending',
        [ApprovedBy] NVARCHAR(MAX) NULL,
        [ApprovalDate] DATETIME2 NULL,
        [DeniedBy] NVARCHAR(MAX) NULL,
        [DenialDate] DATETIME2 NULL,
        [StatusChangedBy] NVARCHAR(MAX) NULL,
        [StatusChangedDate] DATETIME2 NULL,
        [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [EventType] NVARCHAR(100) NULL,
        [StartTime] NVARCHAR(50) NULL,
        [EndTime] NVARCHAR(50) NULL,
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

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'AvEquipmentUsage')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [AvEquipmentUsage] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'SecurityDepositPaid')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [SecurityDepositPaid] BIT NOT NULL DEFAULT 0;

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

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RenterType')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [RenterType] NVARCHAR(50) NOT NULL DEFAULT 'Non-Member';
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'TotalPrice')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [TotalPrice] DECIMAL(18,2) NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'IsPaid')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [IsPaid] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'PaymentMethod')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [PaymentMethod] NVARCHAR(100) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'PaymentDate')
        ALTER TABLE [dbo].[HallRentalRequests] ADD [PaymentDate] DATETIME2 NULL;
        
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
        [Status] NVARCHAR(MAX) NOT NULL, -- Available, Booked, Pending, Blackout
        [Description] NVARCHAR(MAX) NULL,
        [StartTime] NVARCHAR(50) NULL,
        [EndTime] NVARCHAR(50) NULL
    );
    PRINT '✓ Created AvailabilityCalendars table';
END
ELSE
BEGIN
    PRINT 'AvailabilityCalendars table already exists - checking for missing columns...';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AvailabilityCalendars]') AND name = 'Description')
        ALTER TABLE [dbo].[AvailabilityCalendars] ADD [Description] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AvailabilityCalendars]') AND name = 'StartTime')
        ALTER TABLE [dbo].[AvailabilityCalendars] ADD [StartTime] NVARCHAR(50) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AvailabilityCalendars]') AND name = 'EndTime')
        ALTER TABLE [dbo].[AvailabilityCalendars] ADD [EndTime] NVARCHAR(50) NULL;

    PRINT '✓ Verified/Updated AvailabilityCalendars columns';
END
GO

-- 3. Create/Update HallRentalInquiries Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HallRentalInquiries')
BEGIN
    CREATE TABLE [dbo].[HallRentalInquiries] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ResumeToken] NVARCHAR(128) NOT NULL,
        [FormData] NVARCHAR(MAX) NOT NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [ExpiresAt] DATETIME2 NOT NULL
    );
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_HallRentalInquiries_ResumeToken')
        CREATE UNIQUE INDEX IX_HallRentalInquiries_ResumeToken ON [dbo].[HallRentalInquiries]([ResumeToken]);
    PRINT '✓ Created HallRentalInquiries table';
END
ELSE
BEGIN
    PRINT '✓ HallRentalInquiries table already exists';
END
GO

-- 4. Create NotificationRoutings Table (Phase 14 Utility)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NotificationRoutings')
BEGIN
    CREATE TABLE [dbo].[NotificationRoutings] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ActionName] NVARCHAR(100) NOT NULL,
        [DirectorEmail] NVARCHAR(200) NOT NULL
    );
    INSERT INTO [dbo].[NotificationRoutings] (ActionName, DirectorEmail) VALUES ('Rental Inquiry', 'director@gloucesterfraternityclub.com');
    PRINT '✓ Created NotificationRoutings table and added initial routing';
END
ELSE
BEGIN
    PRINT '✓ NotificationRoutings table already exists';
END
GO

-- 5. Create SystemNotifications Table (Phase 11/14 Utility)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SystemNotifications')
BEGIN
    CREATE TABLE [dbo].[SystemNotifications] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [RecipientEmail] NVARCHAR(MAX) NULL,
        [Subject] NVARCHAR(MAX) NULL,
        [Message] NVARCHAR(MAX) NOT NULL,
        [Channel] NVARCHAR(50) NOT NULL,
        [Status] NVARCHAR(50) NULL,
        [SentAt] DATETIME2 NULL,
        [IsActive] BIT NOT NULL DEFAULT 0,
        [StartDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [EndDate] DATETIME2 NULL
    );
    PRINT '✓ Created SystemNotifications table';
END
ELSE
BEGIN
    PRINT '✓ SystemNotifications table already exists';
END
GO

-- 6. Create Remaining Phase 14 Utility Tables
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AssetFolders')
BEGIN
    CREATE TABLE [dbo].[AssetFolders] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL,
        [ParentId] INT NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MediaAssets')
BEGIN
    CREATE TABLE [dbo].[MediaAssets] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FileName] NVARCHAR(255) NOT NULL,
        [FilePath] NVARCHAR(MAX) NOT NULL,
        [ContentType] NVARCHAR(100) NOT NULL,
        [FileSize] BIGINT NOT NULL,
        [FolderId] INT NULL,
        [UploadedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Forms')
BEGIN
    CREATE TABLE [dbo].[Forms] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL,
        [Description] NVARCHAR(MAX) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FormSubmissions')
BEGIN
    CREATE TABLE [dbo].[FormSubmissions] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FormId] INT NOT NULL,
        [SubmissionData] NVARCHAR(MAX) NOT NULL,
        [SubmittedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HallRentals')
BEGIN
    CREATE TABLE [dbo].[HallRentals] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ApplicantName] NVARCHAR(MAX) NOT NULL,
        [ContactInfo] NVARCHAR(MAX) NOT NULL,
        [EventDate] DATETIME2 NOT NULL,
        [Status] NVARCHAR(50) NOT NULL,
        [GuestCount] INT NOT NULL DEFAULT 0,
        [KitchenUsed] BIT NOT NULL DEFAULT 0,
        [TotalPrice] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [InternalNotes] NVARCHAR(MAX) NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MediaRenditions')
BEGIN
    CREATE TABLE [dbo].[MediaRenditions] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [AssetId] INT NOT NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [FilePath] NVARCHAR(MAX) NOT NULL,
        [Width] INT NULL,
        [Height] INT NULL,
        [FileSize] BIGINT NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FormFields')
BEGIN
    CREATE TABLE [dbo].[FormFields] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FormId] INT NOT NULL,
        [Label] NVARCHAR(200) NOT NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [Type] NVARCHAR(50) NOT NULL,
        [IsRequired] BIT NOT NULL DEFAULT 0,
        [Options] NVARCHAR(MAX) NULL,
        [SortOrder] INT NOT NULL DEFAULT 0
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SeoSettings')
BEGIN
    CREATE TABLE [dbo].[SeoSettings] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [PagePath] NVARCHAR(255) NOT NULL,
        [Title] NVARCHAR(200) NULL,
        [Description] NVARCHAR(MAX) NULL,
        [Keywords] NVARCHAR(MAX) NULL,
        [NoIndex] BIT NOT NULL DEFAULT 0
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProtectedDocuments')
BEGIN
    CREATE TABLE [dbo].[ProtectedDocuments] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL,
        [FilePath] NVARCHAR(MAX) NOT NULL,
        [RequiredRole] NVARCHAR(100) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PublicReviews')
BEGIN
    CREATE TABLE [dbo].[PublicReviews] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ReviewerName] NVARCHAR(100) NOT NULL,
        [Rating] INT NOT NULL,
        [Comment] NVARCHAR(MAX) NOT NULL,
        [IsApproved] BIT NOT NULL DEFAULT 0,
        [SubmittedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END

-- 7. Create Studio Tables
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioPages')
BEGIN
    CREATE TABLE StudioPages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Folder NVARCHAR(500) NULL DEFAULT '/',
        Slug NVARCHAR(200) NOT NULL,
        MetaTitle NVARCHAR(200) NULL,
        MetaDescription NVARCHAR(500) NULL,
        OgImage NVARCHAR(500) NULL,
        Status NVARCHAR(20) NOT NULL DEFAULT 'Draft',
        PublishedAt DATETIME2 NULL,
        PublishedBy NVARCHAR(100) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
        IsDeleted BIT NOT NULL DEFAULT 0,
        DeletedAt DATETIME2 NULL,
        DeletedBy NVARCHAR(100) NULL
    );
    PRINT '✓ Created StudioPages table';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioSections')
BEGIN
    CREATE TABLE StudioSections (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        StudioPageId INT NOT NULL,
        Type NVARCHAR(50) NOT NULL,
        OrderIndex INT NOT NULL DEFAULT 0,
        Data NVARCHAR(MAX) NULL,
        AnimationSettingsJson NVARCHAR(MAX) NULL,
        VisibleOnDesktop BIT NOT NULL DEFAULT 1,
        VisibleOnTablet BIT NOT NULL DEFAULT 1,
        VisibleOnMobile BIT NOT NULL DEFAULT 1,
        IsVisible BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
        CONSTRAINT FK_StudioSections_StudioPages FOREIGN KEY (StudioPageId) REFERENCES StudioPages(Id) ON DELETE CASCADE
    );
    PRINT '✓ Created StudioSections table';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioDrafts')
BEGIN
    CREATE TABLE StudioDrafts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        StudioPageId INT NOT NULL,
        ContentSnapshotJson NVARCHAR(MAX) NULL,
        ChangeDescription NVARCHAR(500) NULL,
        Version INT NOT NULL DEFAULT 1,
        IsPublished BIT NOT NULL DEFAULT 0,
        PublishedAt DATETIME2 NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
        CONSTRAINT FK_StudioDrafts_StudioPages FOREIGN KEY (StudioPageId) REFERENCES StudioPages(Id) ON DELETE CASCADE
    );
    PRINT '✓ Created StudioDrafts table';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioSettings')
BEGIN
    CREATE TABLE StudioSettings (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SettingKey NVARCHAR(100) NOT NULL,
        SettingValue NVARCHAR(MAX) NULL,
        Description NVARCHAR(500) NULL,
        LastUpdated DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedBy NVARCHAR(100) NOT NULL DEFAULT 'System'
    );
    CREATE UNIQUE INDEX IX_StudioSettings_SettingKey ON StudioSettings(SettingKey);
    PRINT '✓ Created StudioSettings table';
END

PRINT '-----------------------------------------';
PRINT 'Database fix completed successfully!';
PRINT '-----------------------------------------';
GO
