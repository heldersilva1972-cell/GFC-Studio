-- Add PasswordChangeRequired column to AppUsers table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'PasswordChangeRequired')
BEGIN
    ALTER TABLE [dbo].[AppUsers]
    ADD [PasswordChangeRequired] BIT NOT NULL DEFAULT 0;
    
    PRINT 'PasswordChangeRequired column added successfully';
END
ELSE
BEGIN
    PRINT 'PasswordChangeRequired column already exists';
END
GO
