-- Protocol: Comprehensive Schema Sync for SystemSettings and MagicLinkTokens
-- Created: 2026-01-03
-- Usage: Execute in SSMS to fix "Invalid Column Name" errors by adding ALL potentially missing columns.

BEGIN TRANSACTION;

-- 1. Ensure MagicLinkTokens Table exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MagicLinkTokens]') AND type in (N'U'))
BEGIN
    CREATE TABLE [MagicLinkTokens] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [Token] nvarchar(256) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [ExpiresAtUtc] datetime2 NOT NULL,
        [IsUsed] bit NOT NULL,
        [IpAddress] nvarchar(50) NOT NULL DEFAULT N'',
        CONSTRAINT [PK_MagicLinkTokens] PRIMARY KEY ([Id])
    );
    CREATE INDEX [IX_MagicLinkTokens_Token] ON [MagicLinkTokens] ([Token]);
    PRINT 'Created MagicLinkTokens table.';
END

-- 1.5 Ensure VpnOnboardingTokens Table exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[VpnOnboardingTokens]') AND type in (N'U'))
BEGIN
    CREATE TABLE [VpnOnboardingTokens] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [Token] nvarchar(256) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [ExpiresAtUtc] datetime2 NOT NULL,
        [IsUsed] bit NOT NULL,
        [DeviceInfo] nvarchar(500) NULL,
        CONSTRAINT [PK_VpnOnboardingTokens] PRIMARY KEY ([Id])
    );
    CREATE INDEX [IX_VpnOnboardingTokens_Token] ON [VpnOnboardingTokens] ([Token]);
    PRINT 'Created VpnOnboardingTokens table.';
END

-- 2. SystemSettings - Add ALL potentially missing columns
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'SystemSettings')
BEGIN

    -- VPN / Domain
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PrimaryDomain' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [PrimaryDomain] nvarchar(max) NULL;
        
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'AllowedDomains' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [AllowedDomains] nvarchar(max) NULL;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'DomainSwitchPending' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [DomainSwitchPending] nvarchar(max) NULL;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'DomainSwitchExpiryUtc' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [DomainSwitchExpiryUtc] datetime2 NULL;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'LastConfirmedDomain' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [LastConfirmedDomain] nvarchar(max) NULL;

    -- WireGuard
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'WireGuardPort' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [WireGuardPort] int NOT NULL DEFAULT 51820;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'WireGuardSubnet' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [WireGuardSubnet] nvarchar(max) NOT NULL DEFAULT N'10.8.0.0/24';

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'WireGuardServerPublicKey' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [WireGuardServerPublicKey] nvarchar(max) NULL;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'WireGuardAllowedIPs' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [WireGuardAllowedIPs] nvarchar(max) NOT NULL DEFAULT N'10.8.0.0/24, 192.168.1.0/24';

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MaxSimultaneousViewers' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [MaxSimultaneousViewers] int NOT NULL DEFAULT 10;
        
    -- Security Booleans & Ints
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnableTwoFactorAuth' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [EnableTwoFactorAuth] bit NOT NULL DEFAULT 0;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnableIPFiltering' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [EnableIPFiltering] bit NOT NULL DEFAULT 0;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MinimumBandwidthMbps' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [MinimumBandwidthMbps] int NOT NULL DEFAULT 5;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnableSessionTimeout' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [EnableSessionTimeout] bit NOT NULL DEFAULT 1;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'SessionTimeoutMinutes' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [SessionTimeoutMinutes] int NOT NULL DEFAULT 30;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IdleTimeoutMinutes' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [IdleTimeoutMinutes] int NOT NULL DEFAULT 20;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'AbsoluteSessionMaxMinutes' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [AbsoluteSessionMaxMinutes] int NOT NULL DEFAULT 1440;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnableFailedLoginProtection' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [EnableFailedLoginProtection] bit NOT NULL DEFAULT 1;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MaxFailedLoginAttempts' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [MaxFailedLoginAttempts] int NOT NULL DEFAULT 5;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IPFilterMode' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [IPFilterMode] nvarchar(max) NOT NULL DEFAULT N'Whitelist';

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'LoginLockDurationMinutes' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [LoginLockDurationMinutes] int NOT NULL DEFAULT 30;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'WatermarkPosition' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [WatermarkPosition] nvarchar(max) NOT NULL DEFAULT N'BottomRight';

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnableWatermarking' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [EnableWatermarking] bit NOT NULL DEFAULT 0;

    -- Video Quality
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'LocalQualityMaxBitrate' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [LocalQualityMaxBitrate] int NOT NULL DEFAULT 8000;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'RemoteQualityMaxBitrate' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [RemoteQualityMaxBitrate] int NOT NULL DEFAULT 2000;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnableGeofencing' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [EnableGeofencing] bit NOT NULL DEFAULT 0;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnableConnectionQualityAlerts' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [EnableConnectionQualityAlerts] bit NOT NULL DEFAULT 1;

    -- Director Access
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'DirectorAccessExpiryDate' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [DirectorAccessExpiryDate] datetime2 NULL;

    -- LanSubnet
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'LanSubnet' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [LanSubnet] nvarchar(max) NULL;

    -- Hosting & Security
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'HostingEnvironment' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [HostingEnvironment] nvarchar(20) NOT NULL DEFAULT N'Dev';

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'TrustedDeviceDurationDays' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [TrustedDeviceDurationDays] int NOT NULL DEFAULT 30;

    -- Magic Link & VPN Enforcement (Newest)
    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MagicLinkEnabled' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [MagicLinkEnabled] bit NOT NULL DEFAULT 1;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnforceVpn' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [EnforceVpn] bit NOT NULL DEFAULT 0;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'AccessMode' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [AccessMode] int NOT NULL DEFAULT 0;

    IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EnableOnboarding' AND Object_ID = Object_ID(N'SystemSettings'))
        ALTER TABLE [SystemSettings] ADD [EnableOnboarding] bit NOT NULL DEFAULT 0;

    PRINT 'SystemSettings schema update check complete.';
END

COMMIT;
PRINT 'Comprehensive Database Repair Script Completed.';
