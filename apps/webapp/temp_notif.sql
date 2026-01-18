IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserNotificationPreferences')
BEGIN
    CREATE TABLE UserNotificationPreferences (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        Email NVARCHAR(255),
        Phone NVARCHAR(50),
        ReimbursementNotifyEmail BIT NOT NULL DEFAULT 0,
        ReimbursementNotifySMS BIT NOT NULL DEFAULT 0,
        ReimbursementNotifyPush BIT NOT NULL DEFAULT 0,
        MemberSignupNotifyEmail BIT NOT NULL DEFAULT 0,
        MemberSignupNotifySMS BIT NOT NULL DEFAULT 0,
        MemberSignupNotifyPush BIT NOT NULL DEFAULT 0,
        DuesPaymentNotifyEmail BIT NOT NULL DEFAULT 0,
        DuesPaymentNotifySMS BIT NOT NULL DEFAULT 0,
        DuesPaymentNotifyPush BIT NOT NULL DEFAULT 0,
        SystemAlertNotifyEmail BIT NOT NULL DEFAULT 0,
        SystemAlertNotifySMS BIT NOT NULL DEFAULT 0,
        SystemAlertNotifyPush BIT NOT NULL DEFAULT 0,
        LotterySalesNotifyEmail BIT NOT NULL DEFAULT 0,
        LotterySalesNotifySMS BIT NOT NULL DEFAULT 0,
        LotterySalesNotifyPush BIT NOT NULL DEFAULT 0,
        ControllerEventNotifyEmail BIT NOT NULL DEFAULT 0,
        ControllerEventNotifySMS BIT NOT NULL DEFAULT 0,
        ControllerEventNotifyPush BIT NOT NULL DEFAULT 0,
        NotificationReminderDismissed BIT NOT NULL DEFAULT 0,
        NotificationReminderDismissedAt DATETIME,
        CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_User_Notif FOREIGN KEY (UserId) REFERENCES AppUsers(UserId)
    );
END
ELSE
BEGIN
    -- Add Push columns if they don't exist
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'ReimbursementNotifyPush')
        ALTER TABLE UserNotificationPreferences ADD ReimbursementNotifyPush BIT NOT NULL DEFAULT 0;
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'MemberSignupNotifyPush')
        ALTER TABLE UserNotificationPreferences ADD MemberSignupNotifyPush BIT NOT NULL DEFAULT 0;
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'DuesPaymentNotifyPush')
        ALTER TABLE UserNotificationPreferences ADD DuesPaymentNotifyPush BIT NOT NULL DEFAULT 0;
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'SystemAlertNotifyPush')
        ALTER TABLE UserNotificationPreferences ADD SystemAlertNotifyPush BIT NOT NULL DEFAULT 0;
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'LotterySalesNotifyPush')
        ALTER TABLE UserNotificationPreferences ADD LotterySalesNotifyPush BIT NOT NULL DEFAULT 0;
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'ControllerEventNotifyPush')
        ALTER TABLE UserNotificationPreferences ADD ControllerEventNotifyPush BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PushSubscriptions')
BEGIN
    CREATE TABLE PushSubscriptions (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        Endpoint NVARCHAR(MAX) NOT NULL,
        P256dh NVARCHAR(MAX) NOT NULL,
        Auth NVARCHAR(MAX) NOT NULL,
        DeviceName NVARCHAR(255),
        CreatedAtUtc DATETIME NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_User_Push FOREIGN KEY (UserId) REFERENCES AppUsers(UserId)
    );
END
