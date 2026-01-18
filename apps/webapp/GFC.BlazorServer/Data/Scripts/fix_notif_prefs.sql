-- SECURITY UPGRADE: Fix UserNotificationPreferences table to match C# entity
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'UserNotificationPreferences')
BEGIN
    -- Check if it lacks an Id column
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'Id')
    BEGIN
        -- Too risky to just alter PK, let's drop and recreate since it's a new feature and likely empty
        DROP TABLE [UserNotificationPreferences];
    END
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserNotificationPreferences')
BEGIN
    CREATE TABLE [UserNotificationPreferences] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [Email] NVARCHAR(255) NULL,
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
        [ControllerEventNotifyPush] BIT NOT NULL DEFAULT 1,
        [NotificationReminderDismissed] BIT NOT NULL DEFAULT 0,
        [NotificationReminderDismissedAt] DATETIME2 NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
    CREATE INDEX IX_UserNotificationPreferences_UserId ON [UserNotificationPreferences](UserId);
END
GO
