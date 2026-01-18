-- SECURITY UPGRADE: Passkeys & Push Notifications Schema Fix
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserPasskeys')
BEGIN
    CREATE TABLE [UserPasskeys] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [CredentialId] NVARCHAR(500) NOT NULL,
        [PublicKey] NVARCHAR(MAX) NOT NULL,
        [UserHandle] NVARCHAR(500) NOT NULL,
        [SignatureCounter] INT NOT NULL DEFAULT 0,
        [AttestationFormat] NVARCHAR(50) NULL,
        [FriendlyName] NVARCHAR(200) NULL,
        [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [LastUsedUtc] DATETIME2 NULL,
        [DeviceDisplayName] NVARCHAR(200) NULL,
        [AAGUID] UNIQUEIDENTIFIER NULL
    );
    CREATE INDEX IX_UserPasskeys_UserId ON [UserPasskeys](UserId);
    CREATE INDEX IX_UserPasskeys_CredentialId ON [UserPasskeys](CredentialId);
END
ELSE
BEGIN
    -- Fix columns if they exist but are wrong type or missing
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserPasskeys') AND name = 'FriendlyName')
    BEGIN
        ALTER TABLE [UserPasskeys] ADD [FriendlyName] NVARCHAR(200) NULL;
    END

    -- Ensure these are NVARCHAR if they were VARBINARY from a previous failed attempt
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserPasskeys') AND name = 'CredentialId' AND system_type_id = 165) -- 165 is varbinary
    BEGIN
        ALTER TABLE [UserPasskeys] ALTER COLUMN [CredentialId] NVARCHAR(500) NOT NULL;
    END
    
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserPasskeys') AND name = 'PublicKey' AND system_type_id = 165)
    BEGIN
        ALTER TABLE [UserPasskeys] ALTER COLUMN [PublicKey] NVARCHAR(MAX) NOT NULL;
    END

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserPasskeys') AND name = 'UserHandle' AND system_type_id = 165)
    BEGIN
        ALTER TABLE [UserPasskeys] ALTER COLUMN [UserHandle] NVARCHAR(500) NOT NULL;
    END
END

-- Push Subscriptions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PushSubscriptions')
BEGIN
    CREATE TABLE [PushSubscriptions] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [Endpoint] NVARCHAR(MAX) NOT NULL,
        [P256dh] NVARCHAR(MAX) NOT NULL,
        [Auth] NVARCHAR(MAX) NOT NULL,
        [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
    CREATE INDEX IX_PushSubscriptions_UserId ON [PushSubscriptions](UserId);
END

-- User Notification Preferences
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserNotificationPreferences')
BEGIN
    CREATE TABLE [UserNotificationPreferences] (
        [UserId] INT PRIMARY KEY,
        [Email] NVARCHAR(256) NULL,
        [Phone] NVARCHAR(50) NULL,
        [ReimbursementNotifyEmail] BIT NOT NULL DEFAULT 0,
        [ReimbursementNotifySMS] BIT NOT NULL DEFAULT 0,
        [ReimbursementNotifyPush] BIT NOT NULL DEFAULT 0,
        [MemberSignupNotifyEmail] BIT NOT NULL DEFAULT 0,
        [MemberSignupNotifySMS] BIT NOT NULL DEFAULT 0,
        [MemberSignupNotifyPush] BIT NOT NULL DEFAULT 0,
        [DuesPaymentNotifyEmail] BIT NOT NULL DEFAULT 0,
        [DuesPaymentNotifySMS] BIT NOT NULL DEFAULT 0,
        [DuesPaymentNotifyPush] BIT NOT NULL DEFAULT 0,
        [SystemAlertNotifyEmail] BIT NOT NULL DEFAULT 1,
        [SystemAlertNotifySMS] BIT NOT NULL DEFAULT 1,
        [SystemAlertNotifyPush] BIT NOT NULL DEFAULT 1,
        [LotterySalesNotifyEmail] BIT NOT NULL DEFAULT 0,
        [LotterySalesNotifySMS] BIT NOT NULL DEFAULT 0,
        [LotterySalesNotifyPush] BIT NOT NULL DEFAULT 0,
        [ControllerEventNotifyEmail] BIT NOT NULL DEFAULT 0,
        [ControllerEventNotifySMS] BIT NOT NULL DEFAULT 0,
        [ControllerEventNotifyPush] BIT NOT NULL DEFAULT 1
    );
END
GO
