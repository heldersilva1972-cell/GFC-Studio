# ==============================================================================
# Restore-BingoSession.ps1
# Surgical restore script for May 27th, 2026 Bingo Data.
# Calls sqlcmd directly using native PowerShell invocation with separate -v switches.
# ==============================================================================

$ServerName = ".\SQLEXPRESS"
$DatabaseName = "ClubMembership"
$TempDbName = "ClubMembership_Restore"
$BackupPath = "C:\GFC_Backups\ClubMembership_20260528_020021.bak"
$TargetDate = "2026-05-27"

# Resolve absolute path to the SQL template file located in the same folder as this script
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
if ([string]::IsNullOrEmpty($ScriptDir)) { $ScriptDir = Get-Location }
$SqlTemplatePath = Join-Path $ScriptDir "Restore-BingoSession-Template.sql"

Write-Host "======================================================================" -ForegroundColor Cyan
Write-Host "       BINGO DATA SURGICAL RESTORE TOOL (MAY 27TH, 2026)" -ForegroundColor Cyan
Write-Host "======================================================================" -ForegroundColor Cyan

# 1. Verification
if (-not (Test-Path -Path $BackupPath)) {
    Write-Error "Backup file not found at: $BackupPath"
    Exit
}
if (-not (Test-Path -Path $SqlTemplatePath)) {
    Write-Error "SQL Template file not found at: $SqlTemplatePath"
    Exit
}

Write-Host "[✓] Found backup file at: $BackupPath" -ForegroundColor Green
Write-Host "[✓] Found SQL template file at: $SqlTemplatePath" -ForegroundColor Green

Write-Host "Running surgical restore... please wait..." -ForegroundColor Yellow

# 2. Native PowerShell Execution with Separate -v switches (Perfect Path Handling)
& sqlcmd -S "$ServerName" -i "$SqlTemplatePath" -v BackupPath="$BackupPath" -v TempDbName="$TempDbName" -v DatabaseName="$DatabaseName" -v TargetDate="$TargetDate"

Write-Host "======================================================================" -ForegroundColor Green
Write-Host "             PowerShell Restore Sequence Finished!" -ForegroundColor Green
Write-Host "======================================================================" -ForegroundColor Green
