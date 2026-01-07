# Restore production backup to development LocalDB
# Run this on your LAPTOP

$backupFile = "C:\GFC_Backups\cursor files\ClubMembership_20260106_023150.bak"
$localDbInstance = "(localdb)\MSSQLLocalDB"
$dbName = "ClubMembership"

Write-Host "=== Restoring Production Backup to Development ===" -ForegroundColor Cyan
Write-Host "Backup: $backupFile" -ForegroundColor Gray
Write-Host "Target: $localDbInstance" -ForegroundColor Gray
Write-Host ""

# Check if backup file exists
if (-not (Test-Path $backupFile)) {
    Write-Host "ERROR: Backup file not found at: $backupFile" -ForegroundColor Red
    exit 1
}

Write-Host "[1/3] Backup file found" -ForegroundColor Green
$fileSize = (Get-Item $backupFile).Length / 1MB
Write-Host "      Size: $([math]::Round($fileSize, 2)) MB" -ForegroundColor Gray
Write-Host ""

# Drop existing database if it exists
Write-Host "[2/3] Preparing database..." -ForegroundColor Green
$dropSql = "USE master; IF EXISTS (SELECT name FROM sys.databases WHERE name = N'$dbName') BEGIN ALTER DATABASE [$dbName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$dbName]; END"

try {
    sqlcmd -S $localDbInstance -Q $dropSql -E | Out-Null
    Write-Host "  OK - Ready for restore" -ForegroundColor Green
} catch {
    Write-Host "  FAILED: $_" -ForegroundColor Red
    exit 1
}

# Restore database
Write-Host ""
Write-Host "[3/3] Restoring database (this may take a minute)..." -ForegroundColor Green

# Get logical file names from backup
$fileListSql = "RESTORE FILELISTONLY FROM DISK = N'$backupFile'"
$fileListOutput = sqlcmd -S $localDbInstance -Q $fileListSql -E -h -1 -W

# Parse logical names
$lines = $fileListOutput | Where-Object { $_.Trim() -ne "" }
$logicalDataFile = ($lines[0] -split '\s+')[0]
$logicalLogFile = ($lines[1] -split '\s+')[0]

Write-Host "  Data file: $logicalDataFile" -ForegroundColor Gray
Write-Host "  Log file: $logicalLogFile" -ForegroundColor Gray

# Restore with MOVE to LocalDB default location
$dataPath = "$env:USERPROFILE\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\${dbName}.mdf"
$logPath = "$env:USERPROFILE\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\${dbName}_log.ldf"

$restoreSql = "RESTORE DATABASE [$dbName] FROM DISK = N'$backupFile' WITH MOVE N'$logicalDataFile' TO N'$dataPath', MOVE N'$logicalLogFile' TO N'$logPath', REPLACE, RECOVERY, STATS = 10;"

try {
    sqlcmd -S $localDbInstance -Q $restoreSql -E
    Write-Host "  OK - Database restored successfully" -ForegroundColor Green
} catch {
    Write-Host "  FAILED to restore: $_" -ForegroundColor Red
    exit 1
}

# Verify
Write-Host ""
Write-Host "=== Verification ===" -ForegroundColor Cyan
$verifySql = "SELECT COUNT(*) FROM [$dbName].dbo.Members"
$memberCount = sqlcmd -S $localDbInstance -Q $verifySql -E -h -1 -W

Write-Host "Member count: $($memberCount.Trim())" -ForegroundColor Green

$controllerSql = "SELECT COUNT(*) FROM [$dbName].dbo.Controllers"
$controllerCount = sqlcmd -S $localDbInstance -Q $controllerSql -E -h -1 -W

Write-Host "Controller count: $($controllerCount.Trim())" -ForegroundColor Green

Write-Host ""
Write-Host "SUCCESS! Production data restored to development database." -ForegroundColor Green
Write-Host "You can now run the app in Visual Studio with full production data." -ForegroundColor Green
