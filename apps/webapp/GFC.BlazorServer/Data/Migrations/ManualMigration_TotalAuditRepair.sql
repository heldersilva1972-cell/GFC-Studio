-- Comprehensive Database Health Check and Repair Script
-- This script fixes "Data is Null" errors by ensuring required columns across all Camera-related tables have valid data.

USE [ClubMembership];
GO

PRINT 'Starting comprehensive database repair...';

-- 1. Fix AppUsers Table (User accounts)
PRINT 'Updating AppUsers table...';
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND type in (N'U'))
BEGIN
    UPDATE [dbo].[AppUsers] SET [IsAdmin] = 0 WHERE [IsAdmin] IS NULL;
    UPDATE [dbo].[AppUsers] SET [IsActive] = 1 WHERE [IsActive] IS NULL;
    UPDATE [dbo].[AppUsers] SET [CreatedDate] = GETUTCDATE() WHERE [CreatedDate] IS NULL;
    UPDATE [dbo].[AppUsers] SET [PasswordChangeRequired] = 0 WHERE [PasswordChangeRequired] IS NULL;
    UPDATE [dbo].[AppUsers] SET [Username] = 'unknown_user_' + CAST(UserId AS NVARCHAR(10)) WHERE [Username] IS NULL;
    UPDATE [dbo].[AppUsers] SET [PasswordHash] = '' WHERE [PasswordHash] IS NULL;
END
GO

-- 2. Fix Cameras Table (Camera definitions)
PRINT 'Updating Cameras table...';
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cameras]') AND type in (N'U'))
BEGIN
    UPDATE [dbo].[Cameras] SET [Name] = 'Unnamed Camera' WHERE [Name] IS NULL;
    UPDATE [dbo].[Cameras] SET [RtspUrl] = 'rtsp://0.0.0.0' WHERE [RtspUrl] IS NULL;
    UPDATE [dbo].[Cameras] SET [IsEnabled] = 1 WHERE [IsEnabled] IS NULL;
    UPDATE [dbo].[Cameras] SET [CreatedAt] = GETUTCDATE() WHERE [CreatedAt] IS NULL;
    UPDATE [dbo].[Cameras] SET [UpdatedAt] = GETUTCDATE() WHERE [UpdatedAt] IS NULL;
END
GO

-- 3. Fix CameraAuditLogs Table (The log entries itself)
PRINT 'Updating CameraAuditLogs table...';
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CameraAuditLogs]') AND type in (N'U'))
BEGIN
    UPDATE [dbo].[CameraAuditLogs] SET [CameraId] = 0 WHERE [CameraId] IS NULL;
    UPDATE [dbo].[CameraAuditLogs] SET [UserId] = 0 WHERE [UserId] IS NULL;
    UPDATE [dbo].[CameraAuditLogs] SET [Action] = 'UNKNOWN' WHERE [Action] IS NULL;
    UPDATE [dbo].[CameraAuditLogs] SET [Timestamp] = GETUTCDATE() WHERE [Timestamp] IS NULL;
    UPDATE [dbo].[CameraAuditLogs] SET [Details] = '' WHERE [Details] IS NULL;
END
GO

PRINT 'Database health check and repair completed successfully.';
