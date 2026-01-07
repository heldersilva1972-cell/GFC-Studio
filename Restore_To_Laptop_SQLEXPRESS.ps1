# Simple approach: Use the existing backup file that was already created
# This assumes you have the backup file from earlier at C:\Users\Public\Documents\ClubMembership_Production.bak

$localServer = ".\SQLEXPRESS"
$dbName = "ClubMembership"
$backupFile = "C:\Users\Public\Documents\ClubMembership_Production.bak"

Write-Host "=== Restore Production Database to Laptop SQLEXPRESS ===" -ForegroundColor Cyan
Write-Host ""

# Check if backup file exists
if (-not (Test-Path $backupFile)) {
    Write-Host "ERROR: Backup file not found at: $backupFile" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please do ONE of the following:" -ForegroundColor Yellow
    Write-Host "1. Copy the backup file from the host to: $backupFile" -ForegroundColor Yellow
    Write-Host "2. Update the `$backupFile variable in this script to point to your backup" -ForegroundColor Yellow
    exit 1
}

Write-Host "[1/3] Found backup file: $backupFile" -ForegroundColor Green
$fileSize = (Get-Item $backupFile).Length / 1MB
Write-Host "      Size: $([math]::Round($fileSize, 2)) MB" -ForegroundColor Gray
Write-Host ""

# Step 1: Drop existing database if it exists
Write-Host "[2/3] Preparing local SQLEXPRESS..." -ForegroundColor Green
$dropSql = @"
USE master;
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'$dbName')
BEGIN
    ALTER DATABASE [$dbName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$dbName];
    PRINT 'Dropped existing database';
END
"@

try {
    sqlcmd -S $localServer -Q $dropSql -E | Out-Null
    Write-Host "  ✓ Ready for restore" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Failed: $_" -ForegroundColor Red
    exit 1
}

# Step 2: Restore database
Write-Host "[3/3] Restoring database (this may take a minute)..." -ForegroundColor Green
$restoreSql = @"
RESTORE DATABASE [$dbName]
FROM DISK = N'$backupFile'
WITH REPLACE, RECOVERY, STATS = 10;
"@

try {
    sqlcmd -S $localServer -Q $restoreSql -E
    Write-Host "  ✓ Database restored successfully" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Failed to restore: $_" -ForegroundColor Red
    exit 1
}

# Verify
Write-Host ""
Write-Host "=== Verification ===" -ForegroundColor Cyan
$verifySql = "SELECT COUNT(*) FROM [$dbName].dbo.Members"
$memberCount = sqlcmd -S $localServer -Q $verifySql -E -h -1 -W

Write-Host "Member count: $($memberCount.Trim())" -ForegroundColor Green
Write-Host ""
Write-Host "SUCCESS! You can now run the app in Visual Studio." -ForegroundColor Green
