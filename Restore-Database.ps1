
# ==============================================================================================
# GFC DATABASE RESTORE MANAGER
# ==============================================================================================
# Server: .\SQLEXPRESS
# Database: ClubMembership
# Action: RESTORE database from a selected .bak file
# WARNING: This will OVERWRITE the current database!
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
    # 1. FIND BACKUPS
    Write-Header "SELECT BACKUP TO RESTORE"
    $backupDir = Join-Path $PSScriptRoot "Backups"
    
    if (-not (Test-Path $backupDir)) {
        throw "Backup directory not found at $backupDir. Please run Backup-Database.ps1 first."
    }

    $files = Get-ChildItem -Path $backupDir -Filter "*.bak" | Sort-Object LastWriteTime -Descending
    
    if ($files.Count -eq 0) {
        throw "No .bak files found in $backupDir"
    }

    # List files
    for ($i = 0; $i -lt $files.Count; $i++) {
        $f = $files[$i]
        Write-Host "[$i] $($f.Name)  ($([math]::Round($f.Length/1MB, 2)) MB) - $($f.LastWriteTime)"
    }

    Write-Host ""
    $selection = Read-Host "Enter number to restore (0-$($files.Count - 1)) or 'Q' to quit"
    
    if ($selection -eq 'Q' -or $selection -eq 'q') {
        exit
    }

    $index = $selection -as [int]
    if ($null -eq $index -or $index -lt 0 -or $index -ge $files.Count) {
        throw "Invalid selection."
    }

    $selectedFile = $files[$index]
    Write-Host "Selected: $($selectedFile.FullName)" -ForegroundColor Yellow
    
    Write-Host ""
    Write-Host "WARNING: THIS WILL OVERWRITE THE CURRENT 'ClubMembership' DATABASE!" -ForegroundColor Red
    $confirm = Read-Host "Type 'YES' to confirm restore"
    
    if ($confirm -ne 'YES') {
        Write-Host "Restore cancelled."
        exit
    }

    # 2. PERFORM RESTORE
    Write-Header "RESTORING DATABASE..."
    
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connString
    $conn.Open()

    # We must kick off all other users (including the web app) to restore
    $sql = @"
    USE master;
    
    PRINT 'Disconnecting existing connections...';
    ALTER DATABASE [ClubMembership] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    
    PRINT 'Restoring from backup...';
    RESTORE DATABASE [ClubMembership] 
    FROM DISK = N'$($selectedFile.FullName)' 
    WITH FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10;
    
    PRINT 'Setting back to Multi-User...';
    ALTER DATABASE [ClubMembership] SET MULTI_USER;
"@

    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $sql
    $cmd.CommandTimeout = 300
    
    $cmd.ExecuteNonQuery()
    
    Write-Header "SUCCESS"
    Write-Host "Database restored successfully from: $($selectedFile.Name)" -ForegroundColor Green
    
    $conn.Close()

}
catch {
    Write-Header "ERROR"
    Write-Host "Restore Failed: $_" -ForegroundColor Red
    if ($conn.State -eq 'Open') { $conn.Close() }
}

Write-Host ""
Read-Host "Press Enter to exit..."
