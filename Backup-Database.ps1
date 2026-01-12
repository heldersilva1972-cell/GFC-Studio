
# ==============================================================================================
# GFC DATABASE BACKUP MANAGER
# ==============================================================================================
# Server: .\SQLEXPRESS
# Database: ClubMembership
# Action: Create a full backup (.bak) of the database
# ==============================================================================================

$ErrorActionPreference = "Stop"
$connString = "Server=.\SQLEXPRESS;Database=master;Integrated Security=True;TrustServerCertificate=True;"

function Write-Header {
    param($text)
    Write-Host ""
    Write-Host "==================================================================================" -ForegroundColor Cyan
    Write-Host " $text" -ForegroundColor White
    Write-Host "==================================================================================" -ForegroundColor Cyan
}

try {
    # 1. PREPARE FOLDER
    Write-Header "PREPARING BACKUP FOLDER"
    $backupDir = Join-Path $PSScriptRoot "Backups"
    if (-not (Test-Path $backupDir)) {
        New-Item -ItemType Directory -Force -Path $backupDir | Out-Null
        Write-Host "Created folder: $backupDir" -ForegroundColor Green
    } else {
        Write-Host "Using folder: $backupDir" -ForegroundColor Gray
    }

    # 2. GENERATE FILENAME
    $timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
    $fileName = "ClubMembership_Backup_$timestamp.bak"
    $filePath = Join-Path $backupDir $fileName
    
    # 3. EXECUTE BACKUP
    Write-Header "STARTING BACKUP"
    Write-Host "Target: $filePath" -ForegroundColor Yellow
    
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connString
    $conn.Open()
    
    $backupSql = "BACKUP DATABASE [ClubMembership] TO DISK = N'$filePath' WITH FORMAT, INIT, NAME = N'ClubMembership-Full Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10"
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $backupSql
    
    # Increase timeout for large backups
    $cmd.CommandTimeout = 300 
    
    $cmd.ExecuteNonQuery()
    
    Write-Host "Backup operation completed successfully." -ForegroundColor Green
    
    # 4. VERIFY FILE
    if (Test-Path $filePath) {
        $item = Get-Item $filePath
        $sizeMb = [math]::Round($item.Length / 1MB, 2)
        Write-Header "SUCCESS"
        Write-Host "Backup verified at: $($item.FullName)" -ForegroundColor Green
        Write-Host "Size: $sizeMb MB" -ForegroundColor Green
    } else {
        throw "Backup file was not found after operation."
    }

    $conn.Close()
}
catch {
    Write-Header "ERROR"
    Write-Host "Backup Failed: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "NOTE: If this is a 'Permission Denied' error, it means the SQL Server Service Account" -ForegroundColor Yellow
    Write-Host "does not have permission to write to this folder." -ForegroundColor Yellow
    Write-Host "Try moving this script to C:\Temp or running it as Administrator." -ForegroundColor Yellow
    if ($conn.State -eq 'Open') { $conn.Close() }
}

Write-Host ""
Read-Host "Press Enter to exit..."
