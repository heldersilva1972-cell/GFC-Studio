-- =============================================
-- Key Card System Enhancements - Database Migration
-- Date: 2025-12-29
-- Description: Adds card type tracking, sync queue, and enhanced history
-- =============================================

USE ClubMembership;
GO

-- =============================================
-- 1. Update KeyCards Table
-- =============================================

-- Add CardType column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.KeyCards') AND name = 'CardType')
BEGIN
    ALTER TABLE dbo.KeyCards
    ADD CardType NVARCHAR(10) NULL;

    ALTER TABLE dbo.KeyCards
    ADD IsActive BIT NOT NULL DEFAULT 1;
    
    PRINT 'Added CardType and IsActive columns to KeyCards table';
END
GO

-- Add constraint for CardType
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CHK_CardType')
BEGIN
    ALTER TABLE dbo.KeyCards
    ADD CONSTRAINT CHK_CardType CHECK (CardType IN ('Card', 'Fob'));
    
    PRINT 'Added CardType constraint';
END
GO

-- Create unique index on CardNumber if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UX_KeyCards_CardNumber')
BEGIN
    CREATE UNIQUE INDEX UX_KeyCards_CardNumber 
    ON dbo.KeyCards(CardNumber);
    
    PRINT 'Created unique index on CardNumber';
END
GO

-- =============================================
-- 2. Update MemberKeycardAssignments Table
-- =============================================

-- Add ReasonForChange column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.MemberKeycardAssignments') AND name = 'ReasonForChange')
BEGIN
    ALTER TABLE dbo.MemberKeycardAssignments
    ADD ReasonForChange NVARCHAR(50) NULL;
    
    PRINT 'Added ReasonForChange column to MemberKeycardAssignments table';
END
GO

-- Add DeactivationReason column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.MemberKeycardAssignments') AND name = 'DeactivationReason')
BEGIN
    ALTER TABLE dbo.MemberKeycardAssignments
    ADD DeactivationReason NVARCHAR(50) NULL;
    
    PRINT 'Added DeactivationReason column to MemberKeycardAssignments table';
END
GO

-- Add Notes column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.MemberKeycardAssignments') AND name = 'Notes')
BEGIN
    ALTER TABLE dbo.MemberKeycardAssignments
    ADD Notes NVARCHAR(500) NULL;
    
    PRINT 'Added Notes column to MemberKeycardAssignments table';
END
GO

-- Add constraint for ReasonForChange
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CHK_ReasonForChange')
BEGIN
    ALTER TABLE dbo.MemberKeycardAssignments
    ADD CONSTRAINT CHK_ReasonForChange 
    CHECK (ReasonForChange IN ('Initial', 'Lost', 'Damaged', 'Replaced', 'Stolen', 'DuesUnpaid', 'StatusChange', 'Reactivated', 'MemberInactive', 'MemberSuspended', 'MemberDeceased'));
    
    PRINT 'Added ReasonForChange constraint';
END
GO

-- =============================================
-- 3. Create ControllerSyncQueue Table
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ControllerSyncQueue')
BEGIN
    CREATE TABLE dbo.ControllerSyncQueue (
        QueueId INT IDENTITY(1,1) PRIMARY KEY,
        KeyCardId INT NOT NULL,
        CardNumber NVARCHAR(50) NOT NULL,
        Action NVARCHAR(20) NOT NULL, -- 'ACTIVATE' or 'DEACTIVATE'
        QueuedDate DATETIME NOT NULL DEFAULT GETDATE(),
        AttemptCount INT NOT NULL DEFAULT 0,
        LastAttemptDate DATETIME NULL,
        LastError NVARCHAR(500) NULL,
        Status NVARCHAR(20) NOT NULL DEFAULT 'PENDING', -- 'PENDING', 'PROCESSING', 'COMPLETED'
        CompletedDate DATETIME NULL,
        CONSTRAINT FK_ControllerSyncQueue_KeyCards FOREIGN KEY (KeyCardId) REFERENCES dbo.KeyCards(KeyCardId)
    );
    
    PRINT 'Created ControllerSyncQueue table';
END
GO

-- Create index on Status
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SyncQueue_Status')
BEGIN
    CREATE INDEX IX_SyncQueue_Status ON dbo.ControllerSyncQueue(Status);
    
    PRINT 'Created index on ControllerSyncQueue.Status';
END
GO

-- Create index for next retry calculation
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SyncQueue_NextRetry')
BEGIN
    CREATE INDEX IX_SyncQueue_NextRetry 
    ON dbo.ControllerSyncQueue(LastAttemptDate, AttemptCount) 
    WHERE Status = 'PENDING';
    
    PRINT 'Created index for next retry calculation';
END
GO

-- =============================================
-- 4. Create CardDeactivationLog Table (Optional - for audit trail)
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CardDeactivationLog')
BEGIN
    CREATE TABLE dbo.CardDeactivationLog (
        LogId INT IDENTITY(1,1) PRIMARY KEY,
        KeyCardId INT NOT NULL,
        MemberId INT NOT NULL,
        DeactivatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        Reason NVARCHAR(50) NOT NULL,
        ControllerSynced BIT NOT NULL DEFAULT 0,
        SyncedDate DATETIME NULL,
        Notes NVARCHAR(500) NULL,
        CONSTRAINT FK_CardDeactivationLog_KeyCards FOREIGN KEY (KeyCardId) REFERENCES dbo.KeyCards(KeyCardId),
        CONSTRAINT FK_CardDeactivationLog_Members FOREIGN KEY (MemberId) REFERENCES dbo.Members(MemberID)
    );
    
    PRINT 'Created CardDeactivationLog table';
END
GO

-- =============================================
-- 5. Update existing data with default values
-- =============================================

-- Set default CardType for existing cards (assume 'Card' if not specified)
UPDATE dbo.KeyCards
SET CardType = 'Card'
WHERE CardType IS NULL;
GO

PRINT 'Updated existing KeyCards with default CardType';
GO

-- Set default ReasonForChange for existing assignments
UPDATE dbo.MemberKeycardAssignments
SET ReasonForChange = 'Initial'
WHERE ReasonForChange IS NULL AND ToDate IS NULL;
GO

PRINT 'Updated existing MemberKeycardAssignments with default ReasonForChange';
GO

-- =============================================
-- Migration Complete
-- =============================================

PRINT '';
PRINT '========================================';
PRINT 'Key Card System Enhancements Migration Complete!';
PRINT '========================================';
PRINT '';
PRINT 'Summary:';
PRINT '- Added CardType to KeyCards';
PRINT '- Added ReasonForChange, DeactivationReason, Notes to MemberKeycardAssignments';
PRINT '- Created ControllerSyncQueue table';
PRINT '- Created CardDeactivationLog table';
PRINT '- Updated existing data with defaults';
PRINT '';
GO
