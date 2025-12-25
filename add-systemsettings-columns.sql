USE [ClubMembership]
GO

-- 1. SystemSettings Table Fix
PRINT 'Checking SystemSettings columns...';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'CloudflareTunnelToken')
    ALTER TABLE [dbo].[SystemSettings] ADD [CloudflareTunnelToken] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaxSimultaneousViewers')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaxSimultaneousViewers] INT NULL DEFAULT 5;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PublicDomain')
    ALTER TABLE [dbo].[SystemSettings] ADD [PublicDomain] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardAllowedIPs')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardAllowedIPs] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardPort')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardPort] INT NULL DEFAULT 51820;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardServerPublicKey')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardServerPublicKey] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardSubnet')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardSubnet] NVARCHAR(50) NULL DEFAULT '10.8.0.0/24';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DirectorAccessExpiryDate')
    ALTER TABLE [dbo].[SystemSettings] ADD [DirectorAccessExpiryDate] DATETIME2 NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableConnectionQualityAlerts')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableConnectionQualityAlerts] BIT NOT NULL DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableFailedLoginProtection')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableFailedLoginProtection] BIT NOT NULL DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableGeofencing')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableGeofencing] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableIPFiltering')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableIPFiltering] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableSessionTimeout')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableSessionTimeout] BIT NOT NULL DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableTwoFactorAuth')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableTwoFactorAuth] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableWatermarking')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableWatermarking] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'IPFilterMode')
    ALTER TABLE [dbo].[SystemSettings] ADD [IPFilterMode] NVARCHAR(50) NOT NULL DEFAULT 'Whitelist';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LocalQualityMaxBitrate')
    ALTER TABLE [dbo].[SystemSettings] ADD [LocalQualityMaxBitrate] INT NOT NULL DEFAULT 8000;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LoginLockDurationMinutes')
    ALTER TABLE [dbo].[SystemSettings] ADD [LoginLockDurationMinutes] INT NOT NULL DEFAULT 30;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaxFailedLoginAttempts')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaxFailedLoginAttempts] INT NOT NULL DEFAULT 5;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MinimumBandwidthMbps')
    ALTER TABLE [dbo].[SystemSettings] ADD [MinimumBandwidthMbps] INT NOT NULL DEFAULT 5;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'RemoteQualityMaxBitrate')
    ALTER TABLE [dbo].[SystemSettings] ADD [RemoteQualityMaxBitrate] INT NOT NULL DEFAULT 2000;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SessionTimeoutMinutes')
    ALTER TABLE [dbo].[SystemSettings] ADD [SessionTimeoutMinutes] INT NOT NULL DEFAULT 30;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WatermarkPosition')
    ALTER TABLE [dbo].[SystemSettings] ADD [WatermarkPosition] NVARCHAR(50) NOT NULL DEFAULT 'BottomRight';

-- 2. WebsiteSettings Table Fix
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
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PrimaryColor')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [PrimaryColor] NVARCHAR(20) NOT NULL DEFAULT '#0D1B2A';
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SecondaryColor')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [SecondaryColor] NVARCHAR(20) NOT NULL DEFAULT '#FFD700';
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'HeadingFont')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [HeadingFont] NVARCHAR(100) NOT NULL DEFAULT 'Outfit';
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'BodyFont')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [BodyFont] NVARCHAR(100) NOT NULL DEFAULT 'Inter';
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'HighAccessibilityMode')
        ALTER TABLE [dbo].[WebsiteSettings] ADD [HighAccessibilityMode] BIT NOT NULL DEFAULT 0;
END

-- 3. ProtectedDocuments Table Fix
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

PRINT 'Database fix successfully completed!';
GO
