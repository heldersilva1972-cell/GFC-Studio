-- =============================================
-- Authentication and User Management Tables
-- =============================================
-- Run this script to create the necessary tables for user authentication
-- and user management in the GFC Membership System.

-- AppUsers table: Stores user accounts
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AppUsers] (
        [UserId] INT IDENTITY(1,1) PRIMARY KEY,
        [Username] NVARCHAR(100) NOT NULL UNIQUE,
        [PasswordHash] NVARCHAR(255) NOT NULL,
        [IsAdmin] BIT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [MemberId] INT NULL,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [LastLoginDate] DATETIME NULL,
        [CreatedBy] NVARCHAR(100) NULL,
        [Notes] NVARCHAR(500) NULL,
        [PasswordChangeRequired] BIT NOT NULL DEFAULT 0,
        CONSTRAINT [FK_AppUsers_Members] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Members]([MemberID])
    );
    
    CREATE INDEX [IX_AppUsers_Username] ON [dbo].[AppUsers]([Username]);
    CREATE INDEX [IX_AppUsers_MemberId] ON [dbo].[AppUsers]([MemberId]);
END
GO

-- LoginHistory table: Tracks user login events
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LoginHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LoginHistory] (
        [LoginHistoryId] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [Username] NVARCHAR(100) NOT NULL,
        [LoginDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [IpAddress] NVARCHAR(50) NULL,
        [LoginSuccessful] BIT NOT NULL DEFAULT 1,
        [FailureReason] NVARCHAR(255) NULL,
        CONSTRAINT [FK_LoginHistory_AppUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AppUsers]([UserId])
    );
    
    CREATE INDEX [IX_LoginHistory_UserId] ON [dbo].[LoginHistory]([UserId]);
    CREATE INDEX [IX_LoginHistory_LoginDate] ON [dbo].[LoginHistory]([LoginDate] DESC);
END
GO

-- AuditLogs table: Records administrative security actions
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AuditLogs] (
        [AuditLogId] INT IDENTITY(1,1) PRIMARY KEY,
        [TimestampUtc] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        [PerformedByUserId] INT NULL,
        [TargetUserId] INT NULL,
        [Action] NVARCHAR(100) NOT NULL,
        [Details] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_AuditLogs_PerformedBy] FOREIGN KEY ([PerformedByUserId]) REFERENCES [dbo].[AppUsers]([UserId]),
        CONSTRAINT [FK_AuditLogs_Target] FOREIGN KEY ([TargetUserId]) REFERENCES [dbo].[AppUsers]([UserId])
    );

    CREATE INDEX [IX_AuditLogs_TimestampUtc] ON [dbo].[AuditLogs]([TimestampUtc] DESC);
    CREATE INDEX [IX_AuditLogs_Action] ON [dbo].[AuditLogs]([Action]);
END
GO

-- AppSettings table: Stores application-wide settings
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppSettings]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AppSettings] (
        [SettingKey] NVARCHAR(100) PRIMARY KEY,
        [SettingValue] NVARCHAR(500) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [LastModified] DATETIME NULL,
        [ModifiedBy] NVARCHAR(100) NULL
    );
END
GO

-- Create default admin user (username: admin, password: admin)
-- Password hash is SHA256 of "admin" encoded as Base64
IF NOT EXISTS (SELECT * FROM [dbo].[AppUsers] WHERE [Username] = 'admin')
BEGIN
    INSERT INTO [dbo].[AppUsers] ([Username], [PasswordHash], [IsAdmin], [IsActive], [CreatedDate], [CreatedBy])
    VALUES ('admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 1, 1, GETUTCDATE(), 'System');
END
GO

PRINT 'Authentication tables created successfully!';
PRINT 'Default admin user created: username=admin, password=admin';
PRINT 'Please change the admin password after first login.';

