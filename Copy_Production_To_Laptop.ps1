# Copy Production Database from Host (192.168.1.72) to Laptop SQLEXPRESS
# This script backs up the database on the host and restores it locally

$hostServer = "192.168.1.72\SQLEXPRESS"
$localServer = ".\SQLEXPRESS"
$dbName = "ClubMembership"
$backupPath = "\\192.168.1.72\C$\Temp\ClubMembership_Backup.bak"
$localBackupPath = "C:\Temp\ClubMembership_Backup.bak"

Write-Host "=== Production Database Copy Script ===" -ForegroundColor Cyan
Write-Host "From: $hostServer" -ForegroundColor Yellow
Write-Host "To: $localServer" -ForegroundColor Yellow
Write-Host ""

# Step 1: Create backup on host
Write-Host "[1/4] Creating backup on production host..." -ForegroundColor Green
$backupSql = @"
BACKUP DATABASE [$dbName] 
TO DISK = N'C:\Temp\ClubMembership_Backup.bak' 
WITH FORMAT, INIT, COMPRESSION;
"@

try {
    sqlcmd -S $hostServer -Q $backupSql -E
    Write-Host "  ✓ Backup created successfully" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Failed to create backup: $_" -ForegroundColor Red
    exit 1
}

# Step 2: Copy backup file to laptop
Write-Host "[2/4] Copying backup file to laptop..." -ForegroundColor Green
try {
    if (-not (Test-Path "C:\Temp")) {
        New-Item -Path "C:\Temp" -ItemType Directory | Out-Null
    }
    Copy-Item -Path $backupPath -Destination $localBackupPath -Force
    Write-Host "  ✓ Backup file copied" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Failed to copy backup: $_" -ForegroundColor Red
    Write-Host "  Make sure you have network access to \\192.168.1.72\C$" -ForegroundColor Yellow
    exit 1
}

# Step 3: Restore to local SQLEXPRESS
Write-Host "[3/4] Restoring database to local SQLEXPRESS..." -ForegroundColor Green
$restoreSql = @"
USE master;
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'$dbName')
BEGIN
    ALTER DATABASE [$dbName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$dbName];
END
GO
RESTORE DATABASE [$dbName]
FROM DISK = N'$localBackupPath'
WITH REPLACE, RECOVERY;
GO
"@

try {
    sqlcmd -S $localServer -i $restoreSql -E
    Write-Host "  ✓ Database restored successfully" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Failed to restore database: $_" -ForegroundColor Red
    exit 1
}

# Step 4: Verify
Write-Host "[4/4] Verifying data..." -ForegroundColor Green
$verifySql = "SELECT COUNT(*) as MemberCount FROM [$dbName].dbo.Members"
$result = sqlcmd -S $localServer -Q $verifySql -E -h -1

Write-Host "  ✓ Member count: $($result.Trim())" -ForegroundColor Green
Write-Host ""
Write-Host "=== SUCCESS ===" -ForegroundColor Cyan
Write-Host "Production database copied to laptop SQLEXPRESS" -ForegroundColor Green
Write-Host "You can now run the app locally in Visual Studio" -ForegroundColor Green
