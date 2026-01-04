-- =============================================
-- CRITICAL LOGIN FIX
-- Creates both AppUsers and SystemSettings tables to ensure login works.
-- =============================================

USE [ClubMembership]
GO

-- 1. Create AppUsers (if missing)
PRINT 'Checking AppUsers...';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AppUsers] (
        [UserId] INT IDENTITY(1,1) PRIMARY KEY,
        [Username] NVARCHAR(100) NOT NULL UNIQUE,
        [PasswordHash] NVARCHAR(255) NOT NULL,
        [IsAdmin] BIT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [MemberId] INT NULL,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [LastLoginDate] DATETIME NULL,
        [CreatedBy] NVARCHAR(100) NULL,
        [Notes] NVARCHAR(500) NULL,
        [PasswordChangeRequired] BIT NOT NULL DEFAULT 0,
        
        -- Additional columns needed for authentication features
        [Email] NVARCHAR(255) NULL,
        [MfaEnabled] BIT NOT NULL DEFAULT 0,
        [MfaSecretKey] NVARCHAR(MAX) NULL
    );
    
    -- Create Indexes
    CREATE INDEX [IX_AppUsers_Username] ON [dbo].[AppUsers]([Username]);
    CREATE INDEX [IX_AppUsers_MemberId] ON [dbo].[AppUsers]([MemberId]);

    -- Insert Default Admin
    INSERT INTO [dbo].[AppUsers] ([Username], [PasswordHash], [IsAdmin], [IsActive], [CreatedDate], [CreatedBy])
    VALUES ('admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 1, 1, GETUTCDATE(), 'System');
    PRINT 'Created AppUsers table and Default Admin (admin/admin)';
END
ELSE
BEGIN
    PRINT 'AppUsers table already exists.';
END
GO

-- 2. Create LoginHistory (if missing)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LoginHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LoginHistory] (
        [LoginHistoryId] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NULL,
        [Username] NVARCHAR(100) NULL,
        [LoginDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [IpAddress] NVARCHAR(50) NULL,
        [LoginSuccessful] BIT NOT NULL DEFAULT 1,
        [FailureReason] NVARCHAR(255) NULL
    );
    CREATE INDEX [IX_LoginHistory_UserId] ON [dbo].[LoginHistory]([UserId]);
    PRINT 'Created LoginHistory table.';
END
GO

-- 3. Create AuditLogs (if missing)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AuditLogs] (
        [AuditLogId] INT IDENTITY(1,1) PRIMARY KEY,
        [TimestampUtc] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        [PerformedByUserId] INT NULL,
        [TargetUserId] INT NULL,
        [Action] NVARCHAR(100) NOT NULL,
        [Details] NVARCHAR(MAX) NULL
    );
    CREATE INDEX [IX_AuditLogs_TimestampUtc] ON [dbo].[AuditLogs]([TimestampUtc] DESC);
    PRINT 'Created AuditLogs table.';
END
GO

-- 4. Create SystemSettings (CRITICAL FOR LOGIN)
PRINT 'Checking SystemSettings...';
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SystemSettings')
BEGIN
    CREATE TABLE [dbo].[SystemSettings] (
        [Id] INT NOT NULL PRIMARY KEY,
        [ScannerControllerId] INT NULL,
        [LastUpdatedUtc] DATETIME2 NULL,
        [NvrIpAddress] NVARCHAR(MAX) NULL,
        [NvrPort] INT NULL,
        [NvrUsername] NVARCHAR(MAX) NULL,
        [NvrPassword] NVARCHAR(MAX) NULL,
        [CloudflareTunnelToken] NVARCHAR(MAX) NULL,
        [PrimaryDomain] NVARCHAR(MAX) NULL,
        [AllowedDomains] NVARCHAR(MAX) NULL,
        [DomainSwitchPending] NVARCHAR(MAX) NULL,
        [DomainSwitchExpiryUtc] DATETIME2 NULL,
        [LastConfirmedDomain] NVARCHAR(MAX) NULL,
        [WireGuardPort] INT NOT NULL DEFAULT 51820,
        [WireGuardSubnet] NVARCHAR(MAX) NOT NULL DEFAULT '10.20.0.0/24',
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
        [IPFilterMode] NVARCHAR(MAX) NOT NULL DEFAULT 'Whitelist',
        [LoginLockDurationMinutes] INT NOT NULL DEFAULT 30,
        [WatermarkPosition] NVARCHAR(MAX) NOT NULL DEFAULT 'BottomRight',
        [EnableWatermarking] BIT NOT NULL DEFAULT 0,
        [LocalQualityMaxBitrate] INT NOT NULL DEFAULT 8000,
        [RemoteQualityMaxBitrate] INT NOT NULL DEFAULT 2000,
        [EnableGeofencing] BIT NOT NULL DEFAULT 0,
        [EnableConnectionQualityAlerts] BIT NOT NULL DEFAULT 1,
        [DirectorAccessExpiryDate] DATETIME2 NULL,
        [LanSubnet] NVARCHAR(MAX) NULL DEFAULT '192.168.1.0/24',
        [HostingEnvironment] NVARCHAR(20) NOT NULL DEFAULT 'Dev',
        [TrustedDeviceDurationDays] INT NOT NULL DEFAULT 30,
        [MagicLinkEnabled] BIT NOT NULL DEFAULT 1,
        [EnforceVpn] BIT NOT NULL DEFAULT 0,
        [AccessMode] INT NOT NULL DEFAULT 0,
        [EnableOnboarding] BIT NOT NULL DEFAULT 0,
        [SafeModeEnabled] BIT NOT NULL DEFAULT 0,
        [BackupMethod] NVARCHAR(MAX) NOT NULL DEFAULT 'External USB',
        [LastSuccessfulBackupUtc] DATETIME2 NULL,
        [LastRestoreTestUtc] DATETIME2 NULL,
        [BackupFrequencyHours] INT NOT NULL DEFAULT 24
    );
    INSERT INTO SystemSettings (Id) VALUES (1);
    PRINT 'Created SystemSettings table (Required for Login).';
END
ELSE
BEGIN
    PRINT 'SystemSettings table already exists.';
END
GO

PRINT '========================================';
PRINT 'FIX COMPLETED. TRY LOGGING IN NOW.';
PRINT '========================================';
