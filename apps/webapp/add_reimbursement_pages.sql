INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder) 
VALUES ('Mobile Reimbursements', '/mobile/reimbursements', 'Submit and track your reimbursements on the go.', 'MOBILE', 0, 1, 100);

INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder) 
VALUES ('Mobile Reimbursement Manager', '/mobile/reimbursements/manager', 'Manage and pay pending reimbursements.', 'MOBILE', 1, 1, 101);

-- Also add columns to UserPagePermissions and ReimbursementRequests if they don't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UserPagePermissions') AND name = 'ReceivePush')
BEGIN
    ALTER TABLE UserPagePermissions ADD ReceivePush BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ReimbursementRequests') AND name = 'LastReminderSentUtc')
BEGIN
    ALTER TABLE ReimbursementRequests ADD LastReminderSentUtc DATETIME2 NULL;
END
