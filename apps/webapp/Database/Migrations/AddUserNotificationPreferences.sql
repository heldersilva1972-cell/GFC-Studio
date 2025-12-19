-- =============================================
-- Notification System Database Migration
-- Creates UserNotificationPreferences table
-- =============================================

-- Create UserNotificationPreferences table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserNotificationPreferences')
BEGIN
    CREATE TABLE UserNotificationPreferences (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        Email NVARCHAR(255) NULL,
        Phone NVARCHAR(50) NULL,
        
        -- Reimbursement Notifications
        ReimbursementNotifyEmail BIT NOT NULL DEFAULT 0,
        ReimbursementNotifySMS BIT NOT NULL DEFAULT 0,
        
        -- Member Signup Notifications
        MemberSignupNotifyEmail BIT NOT NULL DEFAULT 0,
        MemberSignupNotifySMS BIT NOT NULL DEFAULT 0,
        
        -- Dues Payment Notifications
        DuesPaymentNotifyEmail BIT NOT NULL DEFAULT 0,
        DuesPaymentNotifySMS BIT NOT NULL DEFAULT 0,
        
        -- System Alerts
        SystemAlertNotifyEmail BIT NOT NULL DEFAULT 0,
        SystemAlertNotifySMS BIT NOT NULL DEFAULT 0,
        
        -- Lottery Sales Notifications
        LotterySalesNotifyEmail BIT NOT NULL DEFAULT 0,
        LotterySalesNotifySMS BIT NOT NULL DEFAULT 0,
        
        -- Controller/Access Control Events
        ControllerEventNotifyEmail BIT NOT NULL DEFAULT 0,
        ControllerEventNotifySMS BIT NOT NULL DEFAULT 0,
        
        -- Reminder tracking
        NotificationReminderDismissed BIT NOT NULL DEFAULT 0,
        NotificationReminderDismissedAt DATETIME2 NULL,
        
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );

    -- Create index on UserId for faster lookups
    CREATE INDEX IX_UserNotificationPreferences_UserId 
    ON UserNotificationPreferences(UserId);

    PRINT 'UserNotificationPreferences table created successfully';
END
ELSE
BEGIN
    PRINT 'UserNotificationPreferences table already exists';
END
GO

-- =============================================
-- Migrate existing reimbursement notification recipients
-- =============================================

-- Check if ReimbursementSettings.NotificationRecipients column exists
IF EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('ReimbursementSettings') 
    AND name = 'NotificationRecipients'
)
BEGIN
    PRINT 'Migrating existing reimbursement notification recipients...';
    
    -- Insert notification preferences for existing recipients
    INSERT INTO UserNotificationPreferences (
        UserId, 
        Email, 
        ReimbursementNotifyEmail, 
        ReimbursementNotifySMS, 
        CreatedAt, 
        UpdatedAt
    )
    SELECT 
        CAST(value AS INT) as UserId,
        NULL as Email, -- Will need to be set manually
        1 as ReimbursementNotifyEmail,
        0 as ReimbursementNotifySMS,
        GETUTCDATE(),
        GETUTCDATE()
    FROM ReimbursementSettings rs
    CROSS APPLY STRING_SPLIT(rs.NotificationRecipients, ',')
    WHERE rs.NotificationRecipients IS NOT NULL
    AND NOT EXISTS (
        SELECT 1 FROM UserNotificationPreferences unp 
        WHERE unp.UserId = CAST(value AS INT)
    );
    
    PRINT 'Migration complete. Existing recipients migrated with email notifications enabled.';
    PRINT 'Note: Email addresses need to be configured manually in User Management.';
END
ELSE
BEGIN
    PRINT 'No NotificationRecipients column found - skipping migration';
END
GO

-- =============================================
-- Optional: Drop old NotificationRecipients column
-- Uncomment when ready to remove old system
-- =============================================

/*
IF EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('ReimbursementSettings') 
    AND name = 'NotificationRecipients'
)
BEGIN
    ALTER TABLE ReimbursementSettings
    DROP COLUMN NotificationRecipients;
    
    PRINT 'NotificationRecipients column dropped from ReimbursementSettings';
END
*/

PRINT 'Notification system migration completed successfully';
GO
