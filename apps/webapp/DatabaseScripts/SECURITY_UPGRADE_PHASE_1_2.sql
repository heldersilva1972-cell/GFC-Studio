IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceInviteTokens]') AND type = N'U')
BEGIN
    CREATE TABLE [dbo].[DeviceInviteTokens](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Token] NVARCHAR(128) NOT NULL,
        [UserId] INT NOT NULL,
        [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        [ExpiresAtUtc] DATETIME2 NOT NULL,
        [UsedAtUtc] DATETIME2 NULL,
        [IsRevoked] BIT NOT NULL DEFAULT 0,
        [TargetDeviceName] NVARCHAR(100) NULL, -- Optional label like "John's iPhone"
        CONSTRAINT [FK_DeviceInviteTokens_AppUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AppUsers]([UserId])
    );

    CREATE UNIQUE INDEX [IX_DeviceInviteTokens_Token] ON [dbo].[DeviceInviteTokens]([Token]);
    CREATE INDEX [IX_DeviceInviteTokens_UserId] ON [dbo].[DeviceInviteTokens]([UserId]);
END
GO

-- Add Page tracking columns to AuditLogs if not exists
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND name = 'PageUrl')
BEGIN
    ALTER TABLE [dbo].[AuditLogs] ADD [PageUrl] NVARCHAR(500) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND name = 'DurationSeconds')
BEGIN
    ALTER TABLE [dbo].[AuditLogs] ADD [DurationSeconds] INT NULL;
END
GO

-- Passkey Storage (Phase 2)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPasskeys]') AND type = N'U')
BEGIN
    CREATE TABLE [dbo].[UserPasskeys](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [CredentialId] VARBINARY(MAX) NOT NULL,
        [PublicKey] VARBINARY(MAX) NOT NULL,
        [UserHandle] VARBINARY(MAX) NOT NULL,
        [SignatureCounter] INT NOT NULL DEFAULT 0,
        [AttestationFormat] NVARCHAR(50) NULL,
        [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        [LastUsedUtc] DATETIME2 NULL,
        [DeviceDisplayName] NVARCHAR(200) NULL,
        [AAGUID] UNIQUEIDENTIFIER NULL,
        CONSTRAINT [FK_UserPasskeys_AppUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AppUsers]([UserId])
    );

    CREATE INDEX [IX_UserPasskeys_UserId] ON [dbo].[UserPasskeys]([UserId]);
END
GO
