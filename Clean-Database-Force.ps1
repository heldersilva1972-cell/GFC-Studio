
# ==============================================================================================
# GFC FORCE CLEAN DATABASE
# ==============================================================================================
# Server: .\SQLEXPRESS
# Database: ClubMembership
# Action: DELETE ALL DATA except Admin Account
# ==============================================================================================

$ErrorActionPreference = "Stop"
$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"

function Write-Header {
    param($text)
    Write-Host ""
    Write-Host "==================================================================================" -ForegroundColor Cyan
    Write-Host " $text" -ForegroundColor White
    Write-Host "==================================================================================" -ForegroundColor Cyan
}

try {
    Write-Header "CONNECTING TO .\SQLEXPRESS"
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connString
    $conn.Open()
    Write-Host "Connected successfully." -ForegroundColor Green

    # 1. CHECK BEFORE
    $cmdCount = $conn.CreateCommand()
    $cmdCount.CommandText = "SELECT COUNT(*) FROM Members"
    $countBefore = $cmdCount.ExecuteScalar()
    Write-Host "Members Found Before Clean: $countBefore" -ForegroundColor Yellow

    if ($countBefore -le 1) {
        Write-Host "Database is already clean. Exiting." -ForegroundColor Green
        Read-Host "Press Enter to exit..."
        exit
    }

    # 2. EXECUTE CLEANUP SQL
    Write-Header "DELETING DATA..."
    
    $cleanSql = @"
    BEGIN TRANSACTION;

    BEGIN TRY
        -- 1. DELETE CHILD RECORDS (Foreign Key Constraints)
        -- We delete anything linked to members to allow member deletion
        
        -- Clear Access Logs/History for Members (keep System logs if any)
        PRINT 'Cleaning MemberHistory...';
        DELETE FROM MemberChangeHistory; 
        
        PRINT 'Cleaning DoorAccess...';
        DELETE FROM MemberDoorAccess;
        
        PRINT 'Cleaning KeycardAssignments...';
        DELETE FROM MemberKeycardAssignments;
        
        -- Also check for Dues/Notes if they exist and are linked
        IF OBJECT_ID('DuesPayments', 'U') IS NOT NULL DELETE FROM DuesPayments;
        IF OBJECT_ID('Dues', 'U') IS NOT NULL DELETE FROM Dues;
        -- GlobalNotes does not have a foreign key to Members, so we don't need to delete from it to clear members.
        -- We can optionally clear it if we want a pristine state, but it won't block deletion.

        PRINT 'Cleaning BoardAssignments...';
        IF OBJECT_ID('BoardAssignments', 'U') IS NOT NULL DELETE FROM BoardAssignments;
        IF OBJECT_ID('BoardMembers', 'U') IS NOT NULL DELETE FROM BoardMembers; -- Just in case

        -- 2. DELETE MEMBERS (Keep ID 1 and 'Admin' users)
        PRINT 'Deleting Members...';
        
        -- Identify Admin ID if possible, otherwise assume ID 1 or secure Keep list
        DECLARE @AdminId INT = (SELECT TOP 1 MemberID FROM Members WHERE FirstName = 'System' OR LastName = 'Admin');
        
        IF @AdminId IS NOT NULL
        BEGIN
            DELETE FROM Members WHERE MemberID <> @AdminId;
        END
        ELSE
        BEGIN
            -- Fallback: Keep ID 1 if it exists, otherwise delete only those with real user data
            DELETE FROM Members WHERE MemberID > 1; 
        END

        COMMIT TRANSACTION;
        PRINT 'SUCCESS: Cleanup Complete.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
"@

    $cmdClean = $conn.CreateCommand()
    $cmdClean.CommandText = $cleanSql
    $cmdClean.ExecuteNonQuery()
    Write-Host "SQL Command Executed." -ForegroundColor Green

    # 3. VERIFY
    $countAfter = $cmdCount.ExecuteScalar()
    Write-Header "VERIFICATION"
    Write-Host "Members Remaining: $countAfter" -ForegroundColor Green

    if ($countAfter -le 1) {
        Write-Host "SUCCESS: Database is now empty." -ForegroundColor Green
    } else {
        Write-Host "WARNING: Some members remain. Check constraints." -ForegroundColor Red
    }

    $conn.Close()
}
catch {
    Write-Host "ERROR: $_" -ForegroundColor Red
    if ($conn.State -eq 'Open') { $conn.Close() }
}

Write-Host ""
Read-Host "Press Enter to exit..."
