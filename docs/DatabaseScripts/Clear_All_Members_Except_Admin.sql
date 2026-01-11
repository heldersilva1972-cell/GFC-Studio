-- DANGEROUS SCRIPT: Clears all Member Data from the database
-- PRESERVES: The Admin Account specified below
-- TARGET: ClubMembership Database
-- VERSION: 6 (Includes AuditLogs cleanup)

USE ClubMembership;
GO

-- !!! IMPORTANT: SET YOUR ADMIN USERNAME HERE !!!
DECLARE @AdminUsername nvarchar(50) = 'admin'; 

DECLARE @AdminUserId int;
SELECT @AdminUserId = UserId FROM AppUsers WHERE Username = @AdminUsername;

-- Safety Check
IF @AdminUserId IS NULL
BEGIN
    PRINT 'ERROR: Admin user ''' + @AdminUsername + ''' not found!';
    PRINT 'Aborting script to prevent locking you out of the system.';
    RETURN;
END

PRINT 'Found Admin User: ' + @AdminUsername + ' (ID: ' + CAST(@AdminUserId as nvarchar(10)) + ')';
PRINT 'Deleting all other members and associated data...';

BEGIN TRANSACTION;

BEGIN TRY
    -- 0. PRE-CLEANUP: Dependent Logs and Queues
    IF OBJECT_ID(N'dbo.ControllerSyncQueue', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning ControllerSyncQueue...';
        DELETE FROM ControllerSyncQueue 
        WHERE KeyCardId IN (
            SELECT KeyCardId FROM KeyCards 
            WHERE MemberId IS NOT NULL AND MemberId <> @AdminUserId
        );
    END

    IF OBJECT_ID(N'dbo.CardDeactivationLog', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning CardDeactivationLog...';
        DELETE FROM CardDeactivationLog
        WHERE KeyCardId IN (
            SELECT KeyCardId FROM KeyCards 
            WHERE MemberId IS NOT NULL AND MemberId <> @AdminUserId
        );
    END
    
    IF OBJECT_ID(N'dbo.LoginHistory', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning LoginHistory...';
        DELETE FROM LoginHistory WHERE UserId <> @AdminUserId;
    END

    IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning AuditLogs...';
        DELETE FROM AuditLogs WHERE PerformedByUserId <> @AdminUserId;
    END

    -- 1. Delete Key Cards assigned to other members
    IF OBJECT_ID(N'dbo.KeyCards', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning KeyCards...';
        DELETE FROM KeyCards WHERE MemberId IS NOT NULL AND MemberId <> @AdminUserId;
    END

    -- 2. Delete Door Access Permissions
    IF OBJECT_ID(N'dbo.MemberDoorAccess', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning MemberDoorAccess...';
        DELETE FROM MemberDoorAccess WHERE MemberId <> @AdminUserId;
    END

    -- 3. Delete Financial/Legal Records
    IF OBJECT_ID(N'dbo.DuesPayments', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning DuesPayments...';
        DELETE FROM DuesPayments WHERE MemberId <> @AdminUserId;
    END

    IF OBJECT_ID(N'dbo.Waivers', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning Waivers...';
        DELETE FROM Waivers WHERE MemberId <> @AdminUserId;
    END

    IF OBJECT_ID(N'dbo.ReimbursementRequests', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning ReimbursementRequests...';
        DELETE FROM ReimbursementRequests WHERE RequestorMemberId <> @AdminUserId;
    END

    -- 4. Delete Queue & History
    IF OBJECT_ID(N'dbo.NPQueueEntries', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning NPQueueEntries...';
        DELETE FROM NPQueueEntries WHERE MemberId <> @AdminUserId;
    END

    IF OBJECT_ID(N'dbo.KeyHistory', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning KeyHistory...';
        DELETE FROM KeyHistory WHERE MemberId <> @AdminUserId;
    END
    
    -- 5. Delete Camera/Stream Access (and other User dependencies)
    IF OBJECT_ID(N'dbo.AuthorizedUsers', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning AuthorizedUsers...';
        DELETE FROM AuthorizedUsers WHERE UserId <> @AdminUserId;
    END

    IF OBJECT_ID(N'dbo.CameraPermissions', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning CameraPermissions...';
        DELETE FROM CameraPermissions WHERE UserId <> @AdminUserId;
    END

    IF OBJECT_ID(N'dbo.CameraAuditLogs', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning CameraAuditLogs...';
        DELETE FROM CameraAuditLogs WHERE UserId <> @AdminUserId;
    END

    IF OBJECT_ID(N'dbo.TrustedDevices', N'U') IS NOT NULL
    BEGIN
        PRINT 'Cleaning TrustedDevices...';
        DELETE FROM TrustedDevices WHERE UserId <> @AdminUserId;
    END

    IF OBJECT_ID(N'dbo.StaffMembers', N'U') IS NOT NULL
    BEGIN
        PRINT 'Unlinking StaffMembers from Users...';
        UPDATE StaffMembers SET MemberId = NULL WHERE MemberId <> @AdminUserId;
    END

    -- 6. Finally, Delete the Users
    PRINT 'Deleting Users...';
    DELETE FROM AppUsers WHERE UserId <> @AdminUserId;

    COMMIT TRANSACTION;
    PRINT 'SUCCESS: Database scrubbed. Only Admin ''' + @AdminUsername + ''' remains.';
    
    -- Optional: Verify results
    SELECT UserId, Username, Email, IsAdmin FROM AppUsers;

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'ERROR ENCOUNTERED: ' + ERROR_MESSAGE();
    PRINT 'Transaction rolled back. No data was deleted.';
END CATCH
