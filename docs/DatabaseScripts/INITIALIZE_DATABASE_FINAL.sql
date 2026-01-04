-- =============================================
-- GFC DATABASE FOUNDATION INITIALIZATION (THE "STOP THE CRASHING" SCRIPT)
-- This script builds ALL core tables required for both Login and Dashboard
-- to prevent "Invalid object name" crashes.
-- =============================================

USE [ClubMembership];
GO

PRINT '========================================';
PRINT 'Initializing GFC Database Foundation';
PRINT '========================================';

-- 1. AppUsers (Security)
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

-- Seed Admin (Password: Admin123!)
IF NOT EXISTS (SELECT 1 FROM [dbo].[AppUsers] WHERE [Username] = 'admin')
BEGIN
    INSERT INTO [dbo].[AppUsers] ([Username], [PasswordHash], [IsAdmin], [IsActive])
    VALUES ('admin', 'eJIaLDaCl5IDkjkQwmiA6oDBC3GUzDhnD15xRjP4bjo=', 1, 1);
    PRINT '✓ Seeded admin user';
END

-- 2. Members (Core)
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

-- 3. BoardPositions (Board)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BoardPositions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[BoardPositions] (
        [PositionID] INT IDENTITY(1,1) NOT NULL,
        [PositionName] NVARCHAR(100) NOT NULL,
        [MaxSeats] INT NOT NULL DEFAULT 1,
        CONSTRAINT [PK_BoardPositions] PRIMARY KEY CLUSTERED ([PositionID] ASC)
    );
    -- Seed default positions
    INSERT INTO [dbo].[BoardPositions] (PositionName, MaxSeats) VALUES ('President', 1), ('Vice President', 1), ('Treasurer', 1), ('Secretary', 1), ('Trustee', 5);
    PRINT '✓ Created BoardPositions table';
END

-- 4. BoardAssignments (Board)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BoardAssignments]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[BoardAssignments] (
        [AssignmentID] INT IDENTITY(1,1) NOT NULL,
        [MemberID] INT NOT NULL,
        [PositionID] INT NOT NULL,
        [TermYear] INT NOT NULL,
        [StartDate] DATETIME2 NULL,
        [EndDate] DATETIME2 NULL,
        [Notes] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_BoardAssignments] PRIMARY KEY CLUSTERED ([AssignmentID] ASC),
        CONSTRAINT [FK_BoardAssignments_Members] FOREIGN KEY ([MemberID]) REFERENCES [dbo].[Members] ([MemberID]),
        CONSTRAINT [FK_BoardAssignments_BoardPositions] FOREIGN KEY ([PositionID]) REFERENCES [dbo].[BoardPositions] ([PositionID])
    );
    PRINT '✓ Created BoardAssignments table';
END

-- 5. DuesYearSettings (Accounting)
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
    PRINT '✓ Created DuesYearSettings table';
END

-- 6. DuesPayments (Accounting)
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

-- 7. NPQueueEntries (Queue)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NPQueueEntries]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[NPQueueEntries] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [MemberId] INT NOT NULL,
        [QueuePosition] INT NOT NULL,
        [JoinedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_NPQueueEntries] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '✓ Created NPQueueEntries table';
END

-- 8. SystemSettings (Config)
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
    PRINT '✓ Created SystemSettings table';
END

-- 9. StaffMembers/Shifts (Staffing)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffMembers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StaffMembers] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(100) NULL,
        [Role] NVARCHAR(50) NULL,
        [MemberId] INT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [PK_StaffMembers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '✓ Created StaffMembers table';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffShifts]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StaffShifts] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [StaffMemberId] INT NOT NULL,
        [Date] DATETIME2 NOT NULL,
        [ShiftType] INT NOT NULL,
        [Status] NVARCHAR(50) NULL,
        [ClockInTime] DATETIME2 NULL,
        [ClockOutTime] DATETIME2 NULL,
        CONSTRAINT [PK_StaffShifts] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '✓ Created StaffShifts table';
END

-- 10. BarSaleEntries (Revenue)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BarSaleEntries]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[BarSaleEntries] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [SaleDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [TotalSales] DECIMAL(18, 2) NOT NULL,
        [TotalItemsSold] INT NOT NULL,
        CONSTRAINT [PK_BarSaleEntries] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '✓ Created BarSaleEntries table';
END

-- 11. Key Management (Security)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KeyCards]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[KeyCards] (
        [KeyCardId] INT IDENTITY(1,1) NOT NULL,
        [MemberID] INT NOT NULL,
        [CardNumber] NVARCHAR(50) NOT NULL,
        [Notes] NVARCHAR(MAX) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CardType] NVARCHAR(20) NOT NULL DEFAULT 'Card',
        [IsControllerSynced] BIT NOT NULL DEFAULT 0,
        [LastControllerSyncDate] DATETIME2 NULL,
        [CreatedDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_KeyCards] PRIMARY KEY CLUSTERED ([KeyCardId] ASC),
        CONSTRAINT [FK_KeyCards_Members] FOREIGN KEY ([MemberID]) REFERENCES [dbo].[Members] ([MemberID])
    );
    PRINT '✓ Created KeyCards table';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemberKeycardAssignments]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[MemberKeycardAssignments] (
        [AssignmentId] INT IDENTITY(1,1) NOT NULL,
        [MemberId] INT NOT NULL,
        [KeyCardId] INT NOT NULL,
        [FromDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [ToDate] DATETIME2 NULL,
        [Reason] NVARCHAR(MAX) NULL,
        [ChangedBy] NVARCHAR(100) NULL,
        CONSTRAINT [PK_MemberKeycardAssignments] PRIMARY KEY CLUSTERED ([AssignmentId] ASC),
        CONSTRAINT [FK_MemberKeycardAssignments_Members] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Members] ([MemberID]),
        CONSTRAINT [FK_MemberKeycardAssignments_KeyCards] FOREIGN KEY ([KeyCardId]) REFERENCES [dbo].[KeyCards] ([KeyCardId])
    );
    PRINT '✓ Created MemberKeycardAssignments table';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhysicalKeys]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PhysicalKeys] (
        [PhysicalKeyID] INT IDENTITY(1,1) NOT NULL,
        [MemberID] INT NOT NULL,
        [IssuedDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [ReturnedDate] DATETIME2 NULL,
        [IssuedBy] NVARCHAR(100) NULL,
        [ReturnedBy] NVARCHAR(100) NULL,
        [Notes] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_PhysicalKeys] PRIMARY KEY CLUSTERED ([PhysicalKeyID] ASC),
        CONSTRAINT [FK_PhysicalKeys_Members] FOREIGN KEY ([MemberID]) REFERENCES [dbo].[Members] ([MemberID])
    );
    PRINT '✓ Created PhysicalKeys table';
END

-- 12. Global Notes & Dues Waivers
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GlobalNotes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[GlobalNotes] (
        [GlobalNoteID] INT IDENTITY(1,1) NOT NULL,
        [NoteDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [Category] NVARCHAR(100) NULL,
        [Text] NVARCHAR(MAX) NOT NULL,
        CONSTRAINT [PK_GlobalNotes] PRIMARY KEY CLUSTERED ([GlobalNoteID] ASC)
    );
    PRINT '✓ Created GlobalNotes table';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DuesWaiverPeriods]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DuesWaiverPeriods] (
        [WaiverId] INT IDENTITY(1,1) NOT NULL,
        [MemberId] INT NOT NULL,
        [StartYear] INT NOT NULL,
        [EndYear] INT NOT NULL,
        [Reason] NVARCHAR(MAX) NOT NULL,
        [CreatedDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [CreatedBy] NVARCHAR(100) NULL,
        CONSTRAINT [PK_DuesWaiverPeriods] PRIMARY KEY CLUSTERED ([WaiverId] ASC),
        CONSTRAINT [FK_DuesWaiverPeriods_Members] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Members] ([MemberID])
    );
    PRINT '✓ Created DuesWaiverPeriods table';
END

-- 13. Logging
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LoginHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LoginHistory] (
        [LoginHistoryId] INT IDENTITY(1,1) NOT NULL,
        [UserId] INT NULL,
        [Username] NVARCHAR(100) NOT NULL,
        [LoginDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [IpAddress] NVARCHAR(50) NULL,
        [LoginSuccessful] BIT NOT NULL,
        [FailureReason] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_LoginHistory] PRIMARY KEY CLUSTERED ([LoginHistoryId] ASC),
        CONSTRAINT [FK_LoginHistory_AppUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AppUsers] ([UserId])
    );
    PRINT '✓ Created LoginHistory table';
END

GO
PRINT '';
PRINT '========================================';
PRINT 'Full Database Foundation Ready!';
PRINT '========================================';
