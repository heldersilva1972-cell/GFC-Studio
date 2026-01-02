# Performance Diagnostic Script
# Run this to identify what's causing the 20-second delay

Write-Host "=== GFC Card Loading Performance Diagnostic ===" -ForegroundColor Cyan
Write-Host ""

# 1. Check if index exists
Write-Host "1. Checking if performance index exists..." -ForegroundColor Yellow
$indexCheck = @"
SELECT name, type_desc 
FROM sys.indexes 
WHERE object_id = OBJECT_ID('dbo.ControllerEvents')
AND name = 'IX_ControllerEvents_CardNumber_TimestampUtc'
"@

try {
    $result = Invoke-Sqlcmd -ServerInstance 'localhost' -Database 'ClubMembership' -Query $indexCheck -QueryTimeout 5
    if ($result) {
        Write-Host "   ✓ Index EXISTS" -ForegroundColor Green
    } else {
        Write-Host "   ✗ Index MISSING - This is the problem!" -ForegroundColor Red
    }
} catch {
    Write-Host "   ✗ Error checking index: $_" -ForegroundColor Red
}

# 2. Check table sizes
Write-Host ""
Write-Host "2. Checking table sizes..." -ForegroundColor Yellow
$sizeCheck = @"
SELECT 
    'KeyCards' as TableName,
    COUNT(*) as RowCount
FROM KeyCards
UNION ALL
SELECT 
    'ControllerEvents',
    COUNT(*)
FROM ControllerEvents
UNION ALL
SELECT 
    'Members',
    COUNT(*)
FROM Members
"@

try {
    $sizes = Invoke-Sqlcmd -ServerInstance 'localhost' -Database 'ClubMembership' -Query $sizeCheck -QueryTimeout 10
    foreach ($row in $sizes) {
        Write-Host "   $($row.TableName): $($row.RowCount) rows" -ForegroundColor White
    }
} catch {
    Write-Host "   ✗ Error checking sizes: $_" -ForegroundColor Red
}

# 3. Test the actual query performance
Write-Host ""
Write-Host "3. Testing query performance..." -ForegroundColor Yellow
$perfTest = @"
SET STATISTICS TIME ON;
DECLARE @start DATETIME = GETDATE();

SELECT CardNumber, MAX(TimestampUtc) as LastUsed
FROM ControllerEvents
WHERE CardNumber IS NOT NULL
GROUP BY CardNumber;

DECLARE @elapsed INT = DATEDIFF(MILLISECOND, @start, GETDATE());
PRINT 'Query took: ' + CAST(@elapsed AS VARCHAR) + 'ms';
SET STATISTICS TIME OFF;
"@

try {
    Write-Host "   Running test query..." -ForegroundColor Gray
    Invoke-Sqlcmd -ServerInstance 'localhost' -Database 'ClubMembership' -Query $perfTest -QueryTimeout 30 -Verbose
} catch {
    Write-Host "   ✗ Query timed out or failed: $_" -ForegroundColor Red
}

# 4. Check for blocking/locks
Write-Host ""
Write-Host "4. Checking for database locks..." -ForegroundColor Yellow
$lockCheck = @"
SELECT 
    request_session_id as SessionID,
    resource_type as ResourceType,
    DB_NAME(resource_database_id) as DatabaseName,
    request_mode as LockMode,
    request_status as Status
FROM sys.dm_tran_locks
WHERE resource_database_id = DB_ID('ClubMembership')
AND request_session_id <> @@SPID
"@

try {
    $locks = Invoke-Sqlcmd -ServerInstance 'localhost' -Database 'ClubMembership' -Query $lockCheck -QueryTimeout 5
    if ($locks) {
        Write-Host "   ⚠ Active locks found:" -ForegroundColor Yellow
        $locks | Format-Table
    } else {
        Write-Host "   ✓ No blocking locks" -ForegroundColor Green
    }
} catch {
    Write-Host "   ✗ Error checking locks: $_" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== Diagnostic Complete ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. If index is missing, run: FIX_CARD_LOADING_PERFORMANCE.sql"
Write-Host "2. If query is still slow (>1000ms), check ControllerEvents table size"
Write-Host "3. If there are locks, restart SQL Server"
Write-Host "4. After fixes, RESTART the Blazor application"
