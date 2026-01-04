-- =============================================
-- GFC DATABASE FOUNDATION INITIALIZATION (FIXED)
-- This script builds ALL core tables required for Login and Dashboard
-- to prevent "Invalid object name" crashes.
-- =============================================

USE [ClubMembership];
GO

PRINT '========================================';
PRINT 'Initializing GFC Database Foundation';
PRINT '========================================';

-- 1. AppUsers (Login) - FIXED SCHEMA
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AppUsers] (
        [UserId] INT IDENTITY(1,1) NOT NULL,
        [Username] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(256) NULL,
        [PasswordHash] NVARCHAR(MAX) NOT NULL,
        [IsAdmin] BIT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [MemberId] INT NULL,
        [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [LastLoginDate] DATETIME2 NULL,
        [CreatedBy] NVARCHAR(100) NULL,
        [Notes] NVARCHAR(500) NULL,
        [PasswordChangeRequired] BIT NOT NULL DEFAULT 0,
        [MfaEnabled] BIT NOT NULL DEFAULT 0,
        [MfaSecretKey] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AppUsers] PRIMARY KEY CLUSTERED ([UserId] ASC)
    );
    CREATE UNIQUE INDEX [IX_AppUsers_Username] ON [dbo].[AppUsers] ([Username]);
    PRINT '✓ Created AppUsers table';
END
ELSE
BEGIN
    -- Ensure columns match the model if table exists
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AppUsers') AND name = 'IsAdmin')
    BEGIN
        ALTER TABLE [dbo].[AppUsers] ADD [IsAdmin] BIT NOT NULL DEFAULT 0;
        PRINT '✓ Added IsAdmin column to AppUsers';
    END
    -- Fix role column if it was accidentally created previously
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AppUsers') AND name = 'Role')
    BEGIN
        ALTER TABLE [dbo].[AppUsers] DROP COLUMN [Role];
        PRINT '✓ Removed legacy Role column from AppUsers';
    END
END

-- Seed Admin if missing (Password: Admin123!)
IF NOT EXISTS (SELECT 1 FROM [dbo].[AppUsers] WHERE [Username] = 'admin')
BEGIN
    INSERT INTO [dbo].[AppUsers] ([Username], [PasswordHash], [IsAdmin], [IsActive])
    VALUES ('admin', 'eJIaLDaCl5IDkjkQwmiA6oDBC3GUzDhnD15xRjP4bjo=', 1, 1);
    PRINT '✓ Seeded admin user';
END
ELSE
BEGIN
    -- Ensure admin has correct password hash (Admin123!) and IsAdmin=1
    UPDATE [dbo].[AppUsers] 
    SET [PasswordHash] = 'eJIaLDaCl5IDkjkQwmiA6oDBC3GUzDhnD15xRjP4bjo=',
        [IsAdmin] = 1,
        [IsActive] = 1
    WHERE [Username] = 'admin';
    PRINT '✓ Reset admin credentials to default (Admin123!)';
END

-- 2. Members (Core Data)
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

-- 3. DuesYearSettings (Accounting Config)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DuesYearSettings]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DuesYearSettings] (
        [Year] INT NOT NULL,
        [StandardDues] DECIMAL(18, 2) NOT NULL DEFAULT 150.00,
        [GraceEndApplied] BIT NOT NULL DEFAULT 0,
        [GraceEndDate] DATETIME2 NULL,
        CONSTRAINT [PK_DuesYearSettings] PRIMARY KEY CLUSTERED ([Year] ASC)
    );
    INSERT INTO [dbo].[DuesYearSettings] ([Year], [StandardDues]) VALUES (YEAR(GETDATE()), 150.00);
    INSERT INTO [dbo].[DuesYearSettings] ([Year], [StandardDues]) VALUES (YEAR(GETDATE())-1, 150.00);
    PRINT '✓ Created DuesYearSettings table and seeded current/previous year';
END

-- 4. DuesPayments (Accounting Records)
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

-- 5. SystemSettings (App Config)
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
    IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemSettings])
    BEGIN
        INSERT INTO [dbo].[SystemSettings] ([Id]) VALUES (1);
    END
    PRINT '✓ Created SystemSettings table';
END

-- 6. Controllers (Hardware)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Controllers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Controllers] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [SerialNumber] INT NOT NULL,
        [IpAddress] NVARCHAR(50) NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [IsEnabled] BIT NOT NULL DEFAULT 1,
        [LastSeenUtc] DATETIME2 NULL,
        CONSTRAINT [PK_Controllers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '✓ Created Controllers table';
END

-- 7. Doors (Hardware)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Doors]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Doors] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [ControllerId] INT NOT NULL,
        [DoorIndex] INT NOT NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [IsEnabled] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Doors] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Doors_Controllers] FOREIGN KEY ([ControllerId]) REFERENCES [dbo].[Controllers] ([Id])
    );
    PRINT '✓ Created Doors table';
END

-- 8. MemberDoorAccess (Security)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemberDoorAccess]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[MemberDoorAccess] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [MemberId] INT NOT NULL,
        [CardNumber] NVARCHAR(50) NOT NULL,
        [DoorId] INT NOT NULL,
        [TimeProfileId] INT NULL,
        [IsEnabled] BIT NOT NULL DEFAULT 1,
        [LastSyncedAt] DATETIME2 NULL,
        [LastSyncResult] NVARCHAR(500) NULL,
        CONSTRAINT [PK_MemberDoorAccess] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_MemberDoorAccess_Doors] FOREIGN KEY ([DoorId]) REFERENCES [dbo].[Doors] ([Id])
    );
    PRINT '✓ Created MemberDoorAccess table';
END

-- 9. ControllerEvents (Audit)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControllerEvents]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ControllerEvents] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [ControllerId] INT NOT NULL,
        [DoorId] INT NULL,
        [TimestampUtc] DATETIME2 NOT NULL,
        [CardNumber] BIGINT NULL,
        [EventType] INT NOT NULL,
        [ReasonCode] INT NULL,
        [IsByCard] BIT NOT NULL DEFAULT 0,
        [IsByButton] BIT NOT NULL DEFAULT 0,
        [RawIndex] INT NOT NULL DEFAULT 0,
        [RawData] NVARCHAR(MAX) NULL,
        [IsSimulated] BIT NOT NULL DEFAULT 0,
        [CreatedUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_ControllerEvents] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '✓ Created ControllerEvents table';
END

-- 10. BarSaleEntries (Revenue)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BarSaleEntries]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[BarSaleEntries] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [SaleDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [TotalSales] DECIMAL(18, 2) NOT NULL,
        [TotalItemsSold] INT NOT NULL,
        [Notes] NVARCHAR(MAX) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_BarSaleEntries] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '✓ Created BarSaleEntries table';
END

-- 11. StaffMembers (HR)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffMembers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StaffMembers] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(100) NULL,
        [Role] NVARCHAR(50) NULL,
        [MemberId] INT NULL,
        [PhoneNumber] NVARCHAR(20) NULL,
        [Email] NVARCHAR(100) NULL,
        [HourlyRate] DECIMAL(18, 2) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [HireDate] DATETIME2 NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NULL,
        [Notes] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_StaffMembers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '✓ Created StaffMembers table';
END

-- 12. StaffShifts (HR)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffShifts]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StaffShifts] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [StaffMemberId] INT NOT NULL,
        [Date] DATETIME2 NOT NULL,
        [ShiftType] INT NOT NULL, -- 1=Day, 2=Night
        [Status] NVARCHAR(50) NULL,
        [ClockInTime] DATETIME2 NULL,
        [ClockOutTime] DATETIME2 NULL,
        CONSTRAINT [PK_StaffShifts] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_StaffShifts_StaffMembers] FOREIGN KEY ([StaffMemberId]) REFERENCES [dbo].[StaffMembers] ([Id])
    );
    PRINT '✓ Created StaffShifts table';
END

GO
PRINT '';
PRINT '========================================';
PRINT 'Full Database Foundation Ready!';
PRINT '========================================';
