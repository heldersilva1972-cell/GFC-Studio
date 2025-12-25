USE [ClubMembership]
GO

-- Add missing columns to SystemSettings table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'CloudflareTunnelToken')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [CloudflareTunnelToken] NVARCHAR(500) NULL;
    PRINT 'Added CloudflareTunnelToken column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaxSimultaneousViewers')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [MaxSimultaneousViewers] INT NULL DEFAULT 5;
    PRINT 'Added MaxSimultaneousViewers column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PublicDomain')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [PublicDomain] NVARCHAR(255) NULL;
    PRINT 'Added PublicDomain column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardAllowedIPs')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardAllowedIPs] NVARCHAR(500) NULL;
    PRINT 'Added WireGuardAllowedIPs column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardPort')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardPort] INT NULL DEFAULT 51820;
    PRINT 'Added WireGuardPort column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardServerPublicKey')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardServerPublicKey] NVARCHAR(500) NULL;
    PRINT 'Added WireGuardServerPublicKey column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardSubnet')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardSubnet] NVARCHAR(50) NULL DEFAULT '10.8.0.0/24';
    PRINT 'Added WireGuardSubnet column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DirectorAccessExpiryDate')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [DirectorAccessExpiryDate] DATETIME2 NULL;
    PRINT 'Added DirectorAccessExpiryDate column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableConnectionQualityAlerts')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableConnectionQualityAlerts] BIT NOT NULL DEFAULT 1;
    PRINT 'Added EnableConnectionQualityAlerts column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableFailedLoginProtection')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableFailedLoginProtection] BIT NOT NULL DEFAULT 1;
    PRINT 'Added EnableFailedLoginProtection column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableGeofencing')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableGeofencing] BIT NOT NULL DEFAULT 0;
    PRINT 'Added EnableGeofencing column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableIPFiltering')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableIPFiltering] BIT NOT NULL DEFAULT 0;
    PRINT 'Added EnableIPFiltering column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableSessionTimeout')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableSessionTimeout] BIT NOT NULL DEFAULT 1;
    PRINT 'Added EnableSessionTimeout column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableTwoFactorAuth')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableTwoFactorAuth] BIT NOT NULL DEFAULT 0;
    PRINT 'Added EnableTwoFactorAuth column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableWatermarking')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableWatermarking] BIT NOT NULL DEFAULT 0;
    PRINT 'Added EnableWatermarking column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'IPFilterMode')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [IPFilterMode] NVARCHAR(50) NOT NULL DEFAULT 'Whitelist';
    PRINT 'Added IPFilterMode column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LocalQualityMaxBitrate')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [LocalQualityMaxBitrate] INT NOT NULL DEFAULT 8000;
    PRINT 'Added LocalQualityMaxBitrate column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LoginLockDurationMinutes')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [LoginLockDurationMinutes] INT NOT NULL DEFAULT 30;
    PRINT 'Added LoginLockDurationMinutes column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaxFailedLoginAttempts')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [MaxFailedLoginAttempts] INT NOT NULL DEFAULT 5;
    PRINT 'Added MaxFailedLoginAttempts column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MinimumBandwidthMbps')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [MinimumBandwidthMbps] INT NOT NULL DEFAULT 5;
    PRINT 'Added MinimumBandwidthMbps column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'RemoteQualityMaxBitrate')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [RemoteQualityMaxBitrate] INT NOT NULL DEFAULT 2000;
    PRINT 'Added RemoteQualityMaxBitrate column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SessionTimeoutMinutes')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SessionTimeoutMinutes] INT NOT NULL DEFAULT 30;
    PRINT 'Added SessionTimeoutMinutes column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WatermarkPosition')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [WatermarkPosition] NVARCHAR(50) NOT NULL DEFAULT 'BottomRight';
    PRINT 'Added WatermarkPosition column';
END

PRINT 'All missing columns have been added successfully!';
GO
