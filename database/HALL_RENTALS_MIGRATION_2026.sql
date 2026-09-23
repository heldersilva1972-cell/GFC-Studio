-- =========================================================================
-- GFC Studio: Hall Rentals, Payment Ledger & Sandbox Schema Migration
-- Database: GFC SQL Server Database
-- Description: Idempotent script to create/update tables for Hall Rentals,
--              Payment Ledger, Dynamic Pricing Matrix, and Sandbox settings.
-- =========================================================================

-- -------------------------------------------------------------------------
-- 1. Create HallRentalPayments Table (Payment Ledger)
-- -------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalPayments]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[HallRentalPayments] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [HallRentalRequestId] INT NOT NULL,
        [PaymentType] NVARCHAR(50) NOT NULL DEFAULT 'Rental Fee',
        [Amount] DECIMAL(18,2) NOT NULL DEFAULT 0.00,
        [PaymentDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [PaymentMethod] NVARCHAR(50) NOT NULL DEFAULT 'Check',
        [ReferenceOrCheckNumber] NVARCHAR(50) NULL,
        [RecordedBy] NVARCHAR(100) NULL,
        [Notes] NVARCHAR(500) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [IsTestPayment] BIT NOT NULL DEFAULT 0
    );
    PRINT 'Created table [dbo].[HallRentalPayments].';
END
ELSE
BEGIN
    PRINT 'Table [dbo].[HallRentalPayments] already exists.';
END
GO

-- -------------------------------------------------------------------------
-- 2. Ensure Foreign Key on HallRentalPayments (if HallRentalRequests exists)
-- -------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND type in (N'U'))
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_HallRentalPayments_HallRentalRequests]') AND parent_object_id = OBJECT_ID(N'[dbo].[HallRentalPayments]'))
    BEGIN
        ALTER TABLE [dbo].[HallRentalPayments]
        ADD CONSTRAINT [FK_HallRentalPayments_HallRentalRequests] 
        FOREIGN KEY ([HallRentalRequestId]) REFERENCES [dbo].[HallRentalRequests] ([Id]) 
        ON DELETE CASCADE;
        PRINT 'Added foreign key [FK_HallRentalPayments_HallRentalRequests].';
    END
END
GO

-- -------------------------------------------------------------------------
-- 3. Update HallRentalRequests Table (Add Missing Columns)
-- -------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND type in (N'U'))
BEGIN
    -- IsTestRecord
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'IsTestRecord')
    BEGIN
        ALTER TABLE [dbo].[HallRentalRequests] ADD [IsTestRecord] BIT NOT NULL DEFAULT 0;
        PRINT 'Added [IsTestRecord] to [dbo].[HallRentalRequests].';
    END

    -- AmountPaid
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'AmountPaid')
    BEGIN
        ALTER TABLE [dbo].[HallRentalRequests] ADD [AmountPaid] DECIMAL(18,2) NOT NULL DEFAULT 0.00;
        PRINT 'Added [AmountPaid] to [dbo].[HallRentalRequests].';
    END

    -- IsPaid
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'IsPaid')
    BEGIN
        ALTER TABLE [dbo].[HallRentalRequests] ADD [IsPaid] BIT NOT NULL DEFAULT 0;
        PRINT 'Added [IsPaid] to [dbo].[HallRentalRequests].';
    END

    -- RequesterAddress
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequesterAddress')
    BEGIN
        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequesterAddress] NVARCHAR(250) NULL;
        PRINT 'Added [RequesterAddress] to [dbo].[HallRentalRequests].';
    END

    -- BartenderRequested
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'BartenderRequested')
    BEGIN
        ALTER TABLE [dbo].[HallRentalRequests] ADD [BartenderRequested] BIT NOT NULL DEFAULT 0;
        PRINT 'Added [BartenderRequested] to [dbo].[HallRentalRequests].';
    END

    -- KitchenUsage
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'KitchenUsage')
    BEGIN
        ALTER TABLE [dbo].[HallRentalRequests] ADD [KitchenUsage] BIT NOT NULL DEFAULT 0;
        PRINT 'Added [KitchenUsage] to [dbo].[HallRentalRequests].';
    END

    -- AvEquipmentUsage
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'AvEquipmentUsage')
    BEGIN
        ALTER TABLE [dbo].[HallRentalRequests] ADD [AvEquipmentUsage] BIT NOT NULL DEFAULT 0;
        PRINT 'Added [AvEquipmentUsage] to [dbo].[HallRentalRequests].';
    END

    -- SecurityDepositAmount
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'SecurityDepositAmount')
    BEGIN
        ALTER TABLE [dbo].[HallRentalRequests] ADD [SecurityDepositAmount] DECIMAL(18,2) NOT NULL DEFAULT 200.00;
        PRINT 'Added [SecurityDepositAmount] to [dbo].[HallRentalRequests].';
    END

    -- SecurityDepositPaid
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'SecurityDepositPaid')
    BEGIN
        ALTER TABLE [dbo].[HallRentalRequests] ADD [SecurityDepositPaid] BIT NOT NULL DEFAULT 0;
        PRINT 'Added [SecurityDepositPaid] to [dbo].[HallRentalRequests].';
    END

    -- InternalNotes
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'InternalNotes')
    BEGIN
        ALTER TABLE [dbo].[HallRentalRequests] ADD [InternalNotes] NVARCHAR(MAX) NULL;
        PRINT 'Added [InternalNotes] to [dbo].[HallRentalRequests].';
    END
END
GO

-- -------------------------------------------------------------------------
-- 4. Update WebsiteSettings Table (Pricing Matrix & Sandbox Configurations)
-- -------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND type in (N'U'))
BEGIN
    -- Rates
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'FunctionHallNonMemberRate')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [FunctionHallNonMemberRate] DECIMAL(18,2) NULL DEFAULT 400.00;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'FunctionHallMemberRate')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [FunctionHallMemberRate] DECIMAL(18,2) NULL DEFAULT 300.00;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'CoalitionNonMemberRate')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [CoalitionNonMemberRate] DECIMAL(18,2) NULL DEFAULT 200.00;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'CoalitionMemberRate')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [CoalitionMemberRate] DECIMAL(18,2) NULL DEFAULT 100.00;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'YouthOrganizationNonMemberRate')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [YouthOrganizationNonMemberRate] DECIMAL(18,2) NULL DEFAULT 100.00;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'YouthOrganizationMemberRate')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [YouthOrganizationMemberRate] DECIMAL(18,2) NULL DEFAULT 100.00;

    -- Add-ons & Deposit
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'BartenderServiceFee')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [BartenderServiceFee] DECIMAL(18,2) NULL DEFAULT 100.00;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'KitchenFee')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [KitchenFee] DECIMAL(18,2) NULL DEFAULT 50.00;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AvEquipmentFee')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [AvEquipmentFee] DECIMAL(18,2) NULL DEFAULT 25.00;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SecurityDepositAmount')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [SecurityDepositAmount] DECIMAL(18,2) NULL DEFAULT 200.00;

    -- Form Intake & Notifications
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'FormIngestionMode')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [FormIngestionMode] NVARCHAR(50) NOT NULL DEFAULT 'NativeForm';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'WebhookApiKey')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [WebhookApiKey] NVARCHAR(100) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NotificationEmailList')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [NotificationEmailList] NVARCHAR(500) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NotifyOnNewSubmission')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [NotifyOnNewSubmission] BIT NOT NULL DEFAULT 1;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NotifyOnPaymentRecorded')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [NotifyOnPaymentRecorded] BIT NOT NULL DEFAULT 1;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SendApplicantConfirmation')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [SendApplicantConfirmation] BIT NOT NULL DEFAULT 1;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NotifyOnStatusChange')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [NotifyOnStatusChange] BIT NOT NULL DEFAULT 1;

    -- Sandbox Configuration
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'EnableSandboxMode')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [EnableSandboxMode] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SandboxCalendarName')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [SandboxCalendarName] NVARCHAR(150) NULL DEFAULT 'GFC Test Rental Calendar';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SandboxTestEmail')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [SandboxTestEmail] NVARCHAR(200) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SandboxGoogleCalendarId')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [SandboxGoogleCalendarId] NVARCHAR(500) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SandboxCalendarFeedUrl')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [SandboxCalendarFeedUrl] NVARCHAR(1000) NULL;

    -- Dynamic Rooms, Add-ons & Deposit Toggle
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RequireSecurityDeposit')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [RequireSecurityDeposit] BIT NOT NULL DEFAULT 1;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RoomsJson')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [RoomsJson] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AddonsJson')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [AddonsJson] NVARCHAR(MAX) NULL;

    -- Form Customizer Content
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalFormTitle')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalFormTitle] NVARCHAR(200) NULL DEFAULT 'Hall Rental Application';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalFormSubtitle')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalFormSubtitle] NVARCHAR(200) NULL DEFAULT 'Gloucester Fraternity Club';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalFormIntroText')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalFormIntroText] NVARCHAR(500) NULL DEFAULT '27 Webster Street, Gloucester, MA 01930 | (978) 283-2889';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalFormRulesText')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalFormRulesText] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalFormSuccessMessage')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalFormSuccessMessage] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'ShowAddressField')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [ShowAddressField] BIT NOT NULL DEFAULT 1;

    -- Backfill default values for existing rows to prevent EF Core Null reader exceptions
    UPDATE [dbo].[WebsiteSettings]
    SET 
        [RequireSecurityDeposit] = ISNULL([RequireSecurityDeposit], 1),
        [RentalFormTitle] = ISNULL([RentalFormTitle], 'Hall Rental Application'),
        [RentalFormSubtitle] = ISNULL([RentalFormSubtitle], 'Gloucester Fraternity Club'),
        [RentalFormIntroText] = ISNULL([RentalFormIntroText], '27 Webster Street, Gloucester, MA 01930 | (978) 283-2889'),
        [RentalFormRulesText] = ISNULL([RentalFormRulesText], 'I have read and agree to the Gloucester Fraternity Club Hall Rental Policy & Rules. I understand that date confirmation is subject to committee approval and receipt of the security deposit.'),
        [RentalFormSuccessMessage] = ISNULL([RentalFormSuccessMessage], 'Your rental request has been received and added to our calendar as Pending. The Hall Rental Committee will review your date and reach out to you directly to confirm booking.'),
        [ShowAddressField] = ISNULL([ShowAddressField], 1),
        [ShowGuestCountField] = ISNULL([ShowGuestCountField], 1),
        [FormIngestionMode] = ISNULL([FormIngestionMode], 'NativeForm'),
        [NotifyOnNewSubmission] = ISNULL([NotifyOnNewSubmission], 1),
        [NotifyOnPaymentRecorded] = ISNULL([NotifyOnPaymentRecorded], 1),
        [SendApplicantConfirmation] = ISNULL([SendApplicantConfirmation], 1),
        [NotifyOnStatusChange] = ISNULL([NotifyOnStatusChange], 1),
        [EnableSandboxMode] = ISNULL([EnableSandboxMode], 0),
        [SecurityDepositAmount] = ISNULL([SecurityDepositAmount], 200.00);

    PRINT 'Updated and backfilled [dbo].[WebsiteSettings] columns.';
END
GO

PRINT '=======================================================';
PRINT ' GFC Studio Hall Rentals migration completed successfully.';
PRINT '=======================================================';
GO
