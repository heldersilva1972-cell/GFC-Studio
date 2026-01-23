IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'PassCodeHash')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [PassCodeHash] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'PasswordChangeRequired')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [PasswordChangeRequired] BIT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TrustedDevices]') AND name = 'DeviceToken')
BEGIN
    -- This would be a larger migration if the table didn't exist, but assuming it matches partial schema
    ALTER TABLE [dbo].[TrustedDevices] ADD [DeviceToken] NVARCHAR(450) NULL;
END
GO
