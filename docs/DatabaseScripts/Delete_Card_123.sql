-- =============================================
-- Fix: Delete Junk Card #123 (Corrected)
-- Description: Permanently removes card #123 and ALL its dependencies.
-- =============================================

USE ClubMembership;
GO

DECLARE @CardId INT;
SELECT @CardId = KeyCardId FROM dbo.KeyCards WHERE CardNumber = '123';

IF @CardId IS NOT NULL
BEGIN
    PRINT 'Found Card #123 (ID: ' + CAST(@CardId AS NVARCHAR(20)) + '). Starting cleanup...';

    BEGIN TRANSACTION;

    BEGIN TRY
        -- 1. Delete from Sync Queue
        DELETE FROM dbo.ControllerSyncQueue WHERE KeyCardId = @CardId;
        PRINT 'Deleted from Sync Queue.';

        -- 2. Delete from Deactivation Log
        DELETE FROM dbo.CardDeactivationLog WHERE KeyCardId = @CardId;
        PRINT 'Deleted from Deactivation Log.';

        -- 3. Delete from MemberKeycardAssignments (The missing step that caused the error)
        DELETE FROM dbo.MemberKeycardAssignments WHERE KeyCardId = @CardId;
        PRINT 'Deleted from MemberKeycardAssignments.';

        -- 4. Delete the KeyCard itself
        DELETE FROM dbo.KeyCards WHERE KeyCardId = @CardId;
        PRINT 'Deleted from KeyCards.';

        COMMIT TRANSACTION;
        PRINT 'Successfully deleted Card #123 and all dependencies.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'Error occurred: ' + ERROR_MESSAGE();
    END CATCH
END
ELSE
BEGIN
    PRINT 'Card #123 not found. It may have already been deleted.';
END
GO
