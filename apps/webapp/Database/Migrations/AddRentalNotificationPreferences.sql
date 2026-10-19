-- Migration: AddRentalNotificationPreferences.sql
-- Granular rental email & push alert toggles per staff user

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'RentalNewSubmissionEmail')
    ALTER TABLE UserNotificationPreferences ADD RentalNewSubmissionEmail BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'RentalNewSubmissionPush')
    ALTER TABLE UserNotificationPreferences ADD RentalNewSubmissionPush BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'RentalInquiryEmail')
    ALTER TABLE UserNotificationPreferences ADD RentalInquiryEmail BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'RentalInquiryPush')
    ALTER TABLE UserNotificationPreferences ADD RentalInquiryPush BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'RentalPaymentEmail')
    ALTER TABLE UserNotificationPreferences ADD RentalPaymentEmail BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'RentalPaymentPush')
    ALTER TABLE UserNotificationPreferences ADD RentalPaymentPush BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'RentalReplyEmail')
    ALTER TABLE UserNotificationPreferences ADD RentalReplyEmail BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserNotificationPreferences') AND name = 'RentalReplyPush')
    ALTER TABLE UserNotificationPreferences ADD RentalReplyPush BIT NOT NULL DEFAULT 0;
