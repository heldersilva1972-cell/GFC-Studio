USE [ClubMembership]
GO

-- 1. SystemSettings Table Repair & Initialization
PRINT 'Starting SystemSettings repair...';

-- Step A: Add columns as NULLable first to avoid insertion errors
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'CloudflareTunnelToken')
    ALTER TABLE [dbo].[SystemSettings] ADD [CloudflareTunnelToken] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaxSimultaneousViewers')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaxSimultaneousViewers] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PublicDomain')
    ALTER TABLE [dbo].[SystemSettings] ADD [PublicDomain] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardAllowedIPs')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardAllowedIPs] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardPort')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardPort] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardServerPublicKey')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardServerPublicKey] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardSubnet')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardSubnet] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LanSubnet')
    ALTER TABLE [dbo].[SystemSettings] ADD [LanSubnet] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrIpAddress')
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrIpAddress] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrPort')
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrPort] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrUsername')
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrUsername] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrPassword')
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrPassword] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'ScannerControllerId')
    ALTER TABLE [dbo].[SystemSettings] ADD [ScannerControllerId] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LastUpdatedUtc')
    ALTER TABLE [dbo].[SystemSettings] ADD [LastUpdatedUtc] DATETIME2 NULL;
GO

-- Step B: Ensure the row with Id=1 exists
IF NOT EXISTS (SELECT * FROM [dbo].[SystemSettings] WHERE Id = 1)
BEGIN
    SET IDENTITY_INSERT [dbo].[SystemSettings] ON;
    INSERT INTO [dbo].[SystemSettings] (Id) VALUES (1);
    SET IDENTITY_INSERT [dbo].[SystemSettings] OFF;
    PRINT 'Created initial SystemSettings row (Id=1)';
END
GO

-- Step C: Now add remaining missing columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DirectorAccessExpiryDate')
    ALTER TABLE [dbo].[SystemSettings] ADD [DirectorAccessExpiryDate] DATETIME2 NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableConnectionQualityAlerts')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableConnectionQualityAlerts] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableFailedLoginProtection')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableFailedLoginProtection] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableGeofencing')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableGeofencing] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableIPFiltering')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableIPFiltering] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableSessionTimeout')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableSessionTimeout] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableTwoFactorAuth')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableTwoFactorAuth] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableWatermarking')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableWatermarking] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'IPFilterMode')
    ALTER TABLE [dbo].[SystemSettings] ADD [IPFilterMode] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LocalQualityMaxBitrate')
    ALTER TABLE [dbo].[SystemSettings] ADD [LocalQualityMaxBitrate] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LoginLockDurationMinutes')
    ALTER TABLE [dbo].[SystemSettings] ADD [LoginLockDurationMinutes] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaxFailedLoginAttempts')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaxFailedLoginAttempts] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MinimumBandwidthMbps')
    ALTER TABLE [dbo].[SystemSettings] ADD [MinimumBandwidthMbps] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'RemoteQualityMaxBitrate')
    ALTER TABLE [dbo].[SystemSettings] ADD [RemoteQualityMaxBitrate] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SessionTimeoutMinutes')
    ALTER TABLE [dbo].[SystemSettings] ADD [SessionTimeoutMinutes] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WatermarkPosition')
    ALTER TABLE [dbo].[SystemSettings] ADD [WatermarkPosition] NVARCHAR(50) NULL;
GO

-- Step D: Populate existing NULLs with defaults
PRINT 'Populating defaults...';
UPDATE [dbo].[SystemSettings] SET
    [MaxSimultaneousViewers] = ISNULL([MaxSimultaneousViewers], 10),
    [WireGuardAllowedIPs] = ISNULL([WireGuardAllowedIPs], '10.8.0.0/24, 192.168.1.0/24'),
    [WireGuardPort] = ISNULL([WireGuardPort], 51820),
    [WireGuardSubnet] = ISNULL([WireGuardSubnet], '10.8.0.0/24'),
    [EnableConnectionQualityAlerts] = ISNULL([EnableConnectionQualityAlerts], 1),
    [EnableFailedLoginProtection] = ISNULL([EnableFailedLoginProtection], 1),
    [EnableGeofencing] = ISNULL([EnableGeofencing], 0),
    [EnableIPFiltering] = ISNULL([EnableIPFiltering], 0),
    [EnableSessionTimeout] = ISNULL([EnableSessionTimeout], 1),
    [EnableTwoFactorAuth] = ISNULL([EnableTwoFactorAuth], 0),
    [EnableWatermarking] = ISNULL([EnableWatermarking], 0),
    [IPFilterMode] = ISNULL([IPFilterMode], 'Whitelist'),
    [LocalQualityMaxBitrate] = ISNULL([LocalQualityMaxBitrate], 8000),
    [LoginLockDurationMinutes] = ISNULL([LoginLockDurationMinutes], 30),
    [MaxFailedLoginAttempts] = ISNULL([MaxFailedLoginAttempts], 5),
    [MinimumBandwidthMbps] = ISNULL([MinimumBandwidthMbps], 5),
    [RemoteQualityMaxBitrate] = ISNULL([RemoteQualityMaxBitrate], 2000),
    [SessionTimeoutMinutes] = ISNULL([SessionTimeoutMinutes], 30),
    [WatermarkPosition] = ISNULL([WatermarkPosition], 'BottomRight'),
    [LanSubnet] = ISNULL([LanSubnet], '192.168.1.0/24')
WHERE Id = 1;
GO

-- Step E: Enforce NOT NULL constraints where required
PRINT 'Enforcing NOT NULL constraints...';
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [MaxSimultaneousViewers] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [WireGuardAllowedIPs] NVARCHAR(500) NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [WireGuardPort] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [WireGuardSubnet] NVARCHAR(50) NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableConnectionQualityAlerts] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableFailedLoginProtection] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableGeofencing] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableIPFiltering] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableSessionTimeout] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableTwoFactorAuth] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableWatermarking] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [IPFilterMode] NVARCHAR(50) NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [LocalQualityMaxBitrate] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [LoginLockDurationMinutes] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [MaxFailedLoginAttempts] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [MinimumBandwidthMbps] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [RemoteQualityMaxBitrate] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [SessionTimeoutMinutes] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [WatermarkPosition] NVARCHAR(50) NOT NULL;

-- 2. WebsiteSettings Table Repair
PRINT 'Checking WebsiteSettings table...';
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'WebsiteSettings')
BEGIN
    CREATE TABLE [dbo].[WebsiteSettings] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ClubPhone] NVARCHAR(MAX) NULL,
        [ClubAddress] NVARCHAR(MAX) NULL,
        [MasterEmailKillSwitch] BIT NOT NULL DEFAULT 0,
        [MemberRate] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [NonMemberRate] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [PrimaryColor] NVARCHAR(20) NOT NULL DEFAULT '#0D1B2A',
        [SecondaryColor] NVARCHAR(20) NOT NULL DEFAULT '#FFD700',
        [HeadingFont] NVARCHAR(100) NOT NULL DEFAULT 'Outfit',
        [BodyFont] NVARCHAR(100) NOT NULL DEFAULT 'Inter',
        [HighAccessibilityMode] BIT NOT NULL DEFAULT 0
    );
END
IF NOT EXISTS (SELECT * FROM [dbo].[WebsiteSettings])
BEGIN
    INSERT INTO [dbo].[WebsiteSettings] (ClubPhone, PrimaryColor) VALUES ('978-283-0507', '#0D1B2A');
END

-- [AUTO-FIX] Ensure new columns exist even if table was already created
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'IsClubOpen')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [IsClubOpen] BIT NOT NULL DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoTitle')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoTitle] NVARCHAR(200) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoDescription')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoDescription] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoKeywords')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoKeywords] NVARCHAR(500) NULL;

-- 3. ProtectedDocuments Table Repair
PRINT 'Checking ProtectedDocuments table...';
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProtectedDocuments')
BEGIN
    CREATE TABLE [dbo].[ProtectedDocuments] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FileName] NVARCHAR(255) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [ContentType] NVARCHAR(100) NOT NULL,
        [FilePath] NVARCHAR(1024) NOT NULL,
        [Visibility] NVARCHAR(50) NOT NULL,
        [UploadedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END

-- 4. AppUsers MFA and Email Columns Fix
PRINT 'Checking AppUsers table for MFA and Email columns...';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'Email')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [Email] NVARCHAR(255) NULL;
    PRINT 'Added Email column to AppUsers';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'MfaEnabled')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [MfaEnabled] BIT NOT NULL DEFAULT 0;
    PRINT 'Added MfaEnabled column to AppUsers';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'MfaSecretKey')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [MfaSecretKey] NVARCHAR(MAX) NULL;
    PRINT 'Added MfaSecretKey column to AppUsers';
END


-- 5. Data Sanitization for WebsiteSettings (Fix 'Data is Null' errors)
PRINT 'Sanitizing WebsiteSettings data...';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WebsiteSettings')
BEGIN
    UPDATE [dbo].[WebsiteSettings] SET [MemberRate] = 0 WHERE [MemberRate] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [NonMemberRate] = 0 WHERE [NonMemberRate] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [IsClubOpen] = 1 WHERE [IsClubOpen] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [MasterEmailKillSwitch] = 0 WHERE [MasterEmailKillSwitch] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [HighAccessibilityMode] = 0 WHERE [HighAccessibilityMode] IS NULL;
    
    -- Ensure colors have defaults if missing
    UPDATE [dbo].[WebsiteSettings] SET [PrimaryColor] = '#0D1B2A' WHERE [PrimaryColor] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [SecondaryColor] = '#FFD700' WHERE [SecondaryColor] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [HeadingFont] = 'Outfit' WHERE [HeadingFont] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [BodyFont] = 'Inter' WHERE [BodyFont] IS NULL;
END
GO

-- 6. Enforce Constraints on WebsiteSettings (Prevent future NULLs)
-- PRINT 'Enforcing constraints on WebsiteSettings...';
-- IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WebsiteSettings')
-- BEGIN
--     ALTER TABLE [dbo].[WebsiteSettings] ALTER COLUMN [MemberRate] DECIMAL(18,2) NOT NULL;
--     ALTER TABLE [dbo].[WebsiteSettings] ALTER COLUMN [NonMemberRate] DECIMAL(18,2) NOT NULL;
--     ALTER TABLE [dbo].[WebsiteSettings] ALTER COLUMN [IsClubOpen] BIT NOT NULL;
--     ALTER TABLE [dbo].[WebsiteSettings] ALTER COLUMN [MasterEmailKillSwitch] BIT NOT NULL;
--     ALTER TABLE [dbo].[WebsiteSettings] ALTER COLUMN [HighAccessibilityMode] BIT NOT NULL;
-- END
-- GO

PRINT 'Database fix successfully completed!';
GO
