-- Add Push Notification columns to SystemSettings if they don't exist
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
