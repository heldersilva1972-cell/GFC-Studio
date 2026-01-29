-- Manual Repair Script for Station Mode Features
-- This script adds missing columns required for Station Mode session tracking

USE [ClubMembership];
GO

-- 1. Add IsStation to TrustedDevices (Required for identifying shared stations)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TrustedDevices]') AND name = N'IsStation')
BEGIN
    ALTER TABLE [dbo].[TrustedDevices] ADD [IsStation] BIT NOT NULL DEFAULT 0;
    PRINT 'Added IsStation column to TrustedDevices';
END

-- 2. Add IsStation to VpnOnboardingTokens (Required for flagging device types during setup)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VpnOnboardingTokens]') AND name = N'IsStation')
BEGIN
    ALTER TABLE [dbo].[VpnOnboardingTokens] ADD [IsStation] BIT NOT NULL DEFAULT 0;
    PRINT 'Added IsStation column to VpnOnboardingTokens';
END

-- 3. Ensure SystemSettings has session control columns (Safeguard)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'AbsoluteSessionMaxMinutes')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [AbsoluteSessionMaxMinutes] INT NOT NULL DEFAULT 1440;
    PRINT 'Added AbsoluteSessionMaxMinutes column to SystemSettings';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'IdleTimeoutMinutes')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [IdleTimeoutMinutes] INT NOT NULL DEFAULT 20;
    PRINT 'Added IdleTimeoutMinutes column to SystemSettings';
END

GO
