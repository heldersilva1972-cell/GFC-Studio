-- =============================================
-- DASHBOARD INITIALIZATION SCRIPT
-- Fixes "Invalid object name" errors for core metrics
-- =============================================

USE [ClubMembership];
GO

PRINT '========================================';
PRINT 'Initializing Dashboard Core Tables';
PRINT '========================================';

-- 1. Create Members Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Members] (
        [MemberID] INT IDENTITY(1,1) NOT NULL,
        [Status] NVARCHAR(50) NOT NULL DEFAULT 'REGULAR',
        [FirstName] NVARCHAR(100) NOT NULL,
        [MiddleName] NVARCHAR(100) NULL,
        [LastName] NVARCHAR(100) NOT NULL,
        [Suffix] NVARCHAR(20) NULL,
        [Address1] NVARCHAR(200) NULL,
        [City] NVARCHAR(100) NULL,
        [State] NVARCHAR(50) NULL,
        [PostalCode] NVARCHAR(20) NULL,
        [Phone] NVARCHAR(20) NULL,
        [CellPhone] NVARCHAR(20) NULL,
        [Email] NVARCHAR(256) NULL,
        [ApplicationDate] DATETIME2 NULL,
        [AcceptedDate] DATETIME2 NULL,
        [StatusChangeDate] DATETIME2 NULL,
        [DateOfBirth] DATETIME2 NULL,
        [Notes] NVARCHAR(MAX) NULL,
        [IsNonPortugueseOrigin] BIT NOT NULL DEFAULT 0,
        [LifeEligibleDate] DATETIME2 NULL,
        [DateOfDeath] DATETIME2 NULL,
        [InactiveDate] DATETIME2 NULL,
        [AddressInvalid] BIT NOT NULL DEFAULT 0,
        [AddressInvalidDate] DATETIME2 NULL,
        [ShowOnWebsite] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED ([MemberID] ASC)
    );
    PRINT '✓ Created Members table';
END

-- 2. Create DuesPayments Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DuesPayments]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DuesPayments] (
        [DuesPaymentID] INT IDENTITY(1,1) NOT NULL,
        [MemberID] INT NOT NULL,
        [Year] INT NOT NULL,
        [Amount] DECIMAL(18, 2) NULL,
        [PaidDate] DATETIME2 NULL,
        [PaymentType] NVARCHAR(50) NULL,
        [Notes] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_DuesPayments] PRIMARY KEY CLUSTERED ([DuesPaymentID] ASC),
        CONSTRAINT [FK_DuesPayments_Members] FOREIGN KEY ([MemberID]) REFERENCES [dbo].[Members] ([MemberID])
    );
    PRINT '✓ Created DuesPayments table';
END

-- 3. Create DuesWaiverPeriods Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DuesWaiverPeriods]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DuesWaiverPeriods] (
        [WaiverId] INT IDENTITY(1,1) NOT NULL,
        [MemberId] INT NOT NULL,
        [StartYear] INT NOT NULL,
        [EndYear] INT NOT NULL,
        [Reason] NVARCHAR(MAX) NULL,
        [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NULL,
        CONSTRAINT [PK_DuesWaiverPeriods] PRIMARY KEY CLUSTERED ([WaiverId] ASC),
        CONSTRAINT [FK_DuesWaiverPeriods_Members] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Members] ([MemberID])
    );
    PRINT '✓ Created DuesWaiverPeriods table';
END

-- 4. Create SystemSettings Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SystemSettings] (
        [Id] INT NOT NULL,
        [ScannerControllerId] INT NULL,
        [LastUpdatedUtc] DATETIME2 NULL,
        [NvrIpAddress] NVARCHAR(50) NULL,
        [NvrPort] INT NULL,
        [NvrUsername] NVARCHAR(100) NULL,
        [NvrPassword] NVARCHAR(MAX) NULL,
        [CloudflareTunnelToken] NVARCHAR(MAX) NULL,
        [PrimaryDomain] NVARCHAR(256) NULL,
        [AllowedDomains] NVARCHAR(MAX) NULL,
        [DomainSwitchPending] NVARCHAR(256) NULL,
        [DomainSwitchExpiryUtc] DATETIME2 NULL,
        [LastConfirmedDomain] NVARCHAR(256) NULL,
        [WireGuardPort] INT NOT NULL DEFAULT 51820,
        [WireGuardSubnet] NVARCHAR(50) NOT NULL DEFAULT '10.20.0.0/24',
        [WireGuardServerPublicKey] NVARCHAR(MAX) NULL,
        [WireGuardAllowedIPs] NVARCHAR(MAX) NOT NULL DEFAULT '10.20.0.0/24',
        [MaxSimultaneousViewers] INT NOT NULL DEFAULT 10,
        [EnableTwoFactorAuth] BIT NOT NULL DEFAULT 0,
        [EnableIPFiltering] BIT NOT NULL DEFAULT 0,
        [MinimumBandwidthMbps] INT NOT NULL DEFAULT 5,
        [EnableSessionTimeout] BIT NOT NULL DEFAULT 1,
        [SessionTimeoutMinutes] INT NOT NULL DEFAULT 30,
        [IdleTimeoutMinutes] INT NOT NULL DEFAULT 20,
        [AbsoluteSessionMaxMinutes] INT NOT NULL DEFAULT 1440,
        [EnableFailedLoginProtection] BIT NOT NULL DEFAULT 1,
        [MaxFailedLoginAttempts] INT NOT NULL DEFAULT 5,
        [IPFilterMode] NVARCHAR(50) NOT NULL DEFAULT 'Whitelist',
        [LoginLockDurationMinutes] INT NOT NULL DEFAULT 30,
        [WatermarkPosition] NVARCHAR(50) NOT NULL DEFAULT 'BottomRight',
        [EnableWatermarking] BIT NOT NULL DEFAULT 0,
        [LocalQualityMaxBitrate] INT NOT NULL DEFAULT 8000,
        [RemoteQualityMaxBitrate] INT NOT NULL DEFAULT 2000,
        [EnableGeofencing] BIT NOT NULL DEFAULT 0,
        [EnableConnectionQualityAlerts] BIT NOT NULL DEFAULT 1,
        [DirectorAccessExpiryDate] DATETIME2 NULL,
        [LanSubnet] NVARCHAR(50) NULL DEFAULT '192.168.1.0/24',
        [HostingEnvironment] NVARCHAR(20) NOT NULL DEFAULT 'Dev',
        [TrustedDeviceDurationDays] INT NOT NULL DEFAULT 30,
        [MagicLinkEnabled] BIT NOT NULL DEFAULT 1,
        [EnforceVpn] BIT NOT NULL DEFAULT 0,
        [AccessMode] INT NOT NULL DEFAULT 0,
        [EnableOnboarding] BIT NOT NULL DEFAULT 0,
        [SafeModeEnabled] BIT NOT NULL DEFAULT 0,
        [BackupMethod] NVARCHAR(100) NULL,
        [LastSuccessfulBackupUtc] DATETIME2 NULL,
        [LastRestoreTestUtc] DATETIME2 NULL,
        [BackupFrequencyHours] INT NOT NULL DEFAULT 24,
        CONSTRAINT [PK_SystemSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    INSERT INTO [dbo].[SystemSettings] ([Id]) VALUES (1);
    PRINT '✓ Created SystemSettings table and seeded default row';
END

GO
PRINT '';
PRINT '========================================';
PRINT 'Dashboard Readiness Check Complete';
PRINT '========================================';
