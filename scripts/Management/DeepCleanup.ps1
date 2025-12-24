# DeepCleanup.ps1
# Finalizes project organization by moving remaining loose scripts and stray folders.

$Root = Get-Location
$ScriptDest = Join-Path $Root "scripts\Launch_Utility"
$ArchiveDest = Join-Path $Root "docs\archive\root_fragments"

# 1. Ensure folders exist
if (!(Test-Path $ScriptDest)) { New-Item -ItemType Directory -Path $ScriptDest -Force | Out-Null }
if (!(Test-Path $ArchiveDest)) { New-Item -ItemType Directory -Path $ArchiveDest -Force | Out-Null }

# 2. Move remaining .bat and .ps1 files 
# (Excludes the cleanup script itself so it can finish running)
Get-ChildItem -Path $Root -Include "*.bat", "*.ps1" -File | Where-Object {
    $_.Name -ne "DeepCleanup.ps1" -and
    $_.Name -ne "FinalProjectCleanup.ps1" -and
    $_.Name -ne "ProjectCleanup.ps1"
} | ForEach-Object {
    Write-Host "Moving Script: $($_.Name)"
    Move-Item -Path $_.FullName -Destination $ScriptDest -Force
}

# 3. Handle the stray GFC.Core folder in root
$strayFolder = Join-Path $Root "GFC.Core"
if (Test-Path $strayFolder) {
    Write-Host "Archiving stray folder: GFC.Core"
    Move-Item -Path $strayFolder -Destination $ArchiveDest -Force
}

Write-Host "`nOrganization complete! Your root directory is now clean." -ForegroundColor Green
