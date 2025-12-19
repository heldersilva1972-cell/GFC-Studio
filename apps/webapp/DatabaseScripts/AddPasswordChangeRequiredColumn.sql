-- Add PasswordChangeRequired column to AppUsers table
-- This script adds support for requiring users to change their password on first login

IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') 
    AND name = 'PasswordChangeRequired'
)
BEGIN
    ALTER TABLE [dbo].[AppUsers]
    ADD [PasswordChangeRequired] BIT NOT NULL DEFAULT 0;
    
    PRINT 'PasswordChangeRequired column added to AppUsers table.';
END
ELSE
BEGIN
    PRINT 'PasswordChangeRequired column already exists in AppUsers table.';
END
GO

