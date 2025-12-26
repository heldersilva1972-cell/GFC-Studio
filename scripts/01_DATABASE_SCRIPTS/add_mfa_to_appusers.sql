-- [NEW]
-- GFC MFA Feature Migration Script
-- Phase 9: Stability, Performance Tuning & User Handover
-- Description: This script adds MFA-related columns to the AppUsers table.

-- Ensure this script is idempotent. It should be safe to run multiple times.

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'MfaEnabled' AND Object_ID = Object_ID(N'dbo.AppUsers'))
BEGIN
    ALTER TABLE dbo.AppUsers
    ADD MfaEnabled BIT NOT NULL DEFAULT 0;
    PRINT 'Column MfaEnabled added to AppUsers table.';
END
ELSE
BEGIN
    PRINT 'Column MfaEnabled already exists in AppUsers table.';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'MfaSecretKey' AND Object_ID = Object_ID(N'dbo.AppUsers'))
BEGIN
    ALTER TABLE dbo.AppUsers
    ADD MfaSecretKey NVARCHAR(MAX) NULL;
    PRINT 'Column MfaSecretKey added to AppUsers table.';
END
ELSE
BEGIN
    PRINT 'Column MfaSecretKey already exists in AppUsers table.';
END
