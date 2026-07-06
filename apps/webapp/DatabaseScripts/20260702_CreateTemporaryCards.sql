-- Create TemporaryCards Table and alter CardDeactivationLog MemberId to NULL
-- Also make KeyCardId in ControllerSyncQueue nullable
-- Also make KeyCardId in CardDeactivationLog nullable
-- Run this on ClubMembership database

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TemporaryCards')
BEGIN
    CREATE TABLE dbo.TemporaryCards (
        TemporaryCardId INT IDENTITY(1,1) PRIMARY KEY,
        CardNumber NVARCHAR(50) NULL,
        HolderName NVARCHAR(100) NOT NULL,
        Purpose NVARCHAR(200) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 0,
        ActiveFrom DATETIME NOT NULL,
        ActiveTo DATETIME NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Created TemporaryCards table';
END
ELSE
BEGIN
    PRINT 'TemporaryCards table already exists';
END

-- Alter CardDeactivationLog to make MemberId and KeyCardId nullable
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.CardDeactivationLog') AND name = 'MemberId')
BEGIN
    IF (SELECT is_nullable FROM sys.columns WHERE object_id = OBJECT_ID('dbo.CardDeactivationLog') AND name = 'MemberId') = 0
    BEGIN
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CardDeactivationLog_Members')
        BEGIN
            ALTER TABLE dbo.CardDeactivationLog DROP CONSTRAINT FK_CardDeactivationLog_Members;
            PRINT 'Dropped FK_CardDeactivationLog_Members constraint';
        END

        ALTER TABLE dbo.CardDeactivationLog ALTER COLUMN MemberId INT NULL;
        PRINT 'Altered MemberId column to be NULL';

        ALTER TABLE dbo.CardDeactivationLog ADD CONSTRAINT FK_CardDeactivationLog_Members FOREIGN KEY (MemberId) REFERENCES dbo.Members(MemberID);
        PRINT 'Recreated FK_CardDeactivationLog_Members constraint';
    END
END

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.CardDeactivationLog') AND name = 'KeyCardId')
BEGIN
    IF (SELECT is_nullable FROM sys.columns WHERE object_id = OBJECT_ID('dbo.CardDeactivationLog') AND name = 'KeyCardId') = 0
    BEGIN
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CardDeactivationLog_KeyCards')
        BEGIN
            ALTER TABLE dbo.CardDeactivationLog DROP CONSTRAINT FK_CardDeactivationLog_KeyCards;
            PRINT 'Dropped FK_CardDeactivationLog_KeyCards constraint';
        END

        ALTER TABLE dbo.CardDeactivationLog ALTER COLUMN KeyCardId INT NULL;
        PRINT 'Altered KeyCardId column to be NULL';

        ALTER TABLE dbo.CardDeactivationLog ADD CONSTRAINT FK_CardDeactivationLog_KeyCards FOREIGN KEY (KeyCardId) REFERENCES dbo.KeyCards(KeyCardId);
        PRINT 'Recreated FK_CardDeactivationLog_KeyCards constraint';
    END
END

-- Alter ControllerSyncQueue to make KeyCardId nullable
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ControllerSyncQueue') AND name = 'KeyCardId')
BEGIN
    IF (SELECT is_nullable FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ControllerSyncQueue') AND name = 'KeyCardId') = 0
    BEGIN
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ControllerSyncQueue_KeyCards')
        BEGIN
            ALTER TABLE dbo.ControllerSyncQueue DROP CONSTRAINT FK_ControllerSyncQueue_KeyCards;
            PRINT 'Dropped FK_ControllerSyncQueue_KeyCards constraint';
        END

        ALTER TABLE dbo.ControllerSyncQueue ALTER COLUMN KeyCardId INT NULL;
        PRINT 'Altered KeyCardId column to be NULL';

        ALTER TABLE dbo.ControllerSyncQueue ADD CONSTRAINT FK_ControllerSyncQueue_KeyCards FOREIGN KEY (KeyCardId) REFERENCES dbo.KeyCards(KeyCardId);
        PRINT 'Recreated FK_ControllerSyncQueue_KeyCards constraint';
    END
END
