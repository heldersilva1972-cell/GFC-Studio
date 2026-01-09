-- Manual Repair Script for AppUsers Table
-- This script adds missing MFA columns for user accounts

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = N'MfaEnabled')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [MfaEnabled] BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = N'MfaSecretKey')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [MfaSecretKey] NVARCHAR(MAX) NULL;
END

GO
