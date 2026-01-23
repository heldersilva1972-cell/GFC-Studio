-- FIX: Add missing Communication columns to SystemSettings
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'EmailEnabled')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [EmailEnabled] BIT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'SmsEnabled')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SmsEnabled] BIT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'PreferredMagicLinkMethod')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [PreferredMagicLinkMethod] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'TwilioAccountSid')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioAccountSid] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'TwilioAuthToken')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioAuthToken] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'TwilioFromNumber')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioFromNumber] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'SmtpHost')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpHost] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'SmtpPort')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpPort] INT NOT NULL DEFAULT 587;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'SmtpUsername')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpUsername] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'SmtpPassword')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpPassword] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'SmtpEnableSsl')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpEnableSsl] BIT NOT NULL DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'SmtpFromAddress')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpFromAddress] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'SmtpFromName')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpFromName] NVARCHAR(MAX) NULL;
END
GO

-- Also ensure Push columns are there (just in case)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'PushEnabled')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [PushEnabled] BIT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'VapidPublicKey')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [VapidPublicKey] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'VapidPrivateKey')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [VapidPrivateKey] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'VapidSubject')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [VapidSubject] NVARCHAR(MAX) NULL;
END
GO
