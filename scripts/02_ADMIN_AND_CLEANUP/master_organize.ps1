# GFC Master Documentation & System Organizer V3
# This script precisely moves files between complete and in-process.

$docsPath = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\docs"
$historyPath = "$docsPath\History_Logs"
$completePath = "$docsPath\complete"
$inProcessPath = "$docsPath\in-process"

Write-Host "--- Organizing GFC Documentation ---" -ForegroundColor Cyan

# 1. Move Completed items from In-Process to Complete
# Roadmap confirms Phase 0-6 and Phase 10 are completed/baseline work.
$completedPatterns = @("*PHASE_([0-6]|10)*", "*COMPLETE*", "*SUMMARY*")
Get-ChildItem -Path $inProcessPath -File | ForEach-Object {
    if ($_.Name -match "PHASE_([0-6]|10)" -or $_.Name -like "*COMPLETE*" -or $_.Name -like "*SUMMARY*") {
        Write-Host "Completing: $($_.Name) -> docs\complete"
        Move-Item -Path $_.FullName -Destination $completePath -Force
    }
}

# 2. Targeted Sort: Move Active Work from History_Logs to proper Phase Folders
$phase7Dir = "$inProcessPath\PHASE_7_MOBILE"
if (!(Test-Path $phase7Dir)) { New-Item -ItemType Directory -Path $phase7Dir | Out-Null }

$phase7Files = @(
    "BARTENDER_SCHEDULE_ENHANCEMENTS.md",
    "STAFF_MANAGEMENT_IMPLEMENTATION_SUMMARY.md",
    "SHIFT_ASSIGNMENT_DEBUG_GUIDE.md"
)

foreach ($f in $phase7Files) {
    if (Test-Path "$historyPath\$f") {
        Write-Host "Targeting: $f -> Phase 7 (Mobile/Staff)"
        Move-Item -Path "$historyPath\$f" -Destination $phase7Dir -Force
    }
}

# 3. Targeted Sort: Move Website Setup to Phase 11
$phase11Dir = "$inProcessPath\PHASE_11_NEW_GFC_WEBSITE"
$websiteFiles = @(
    "HOW_TO_START_WEBSITE.txt",
    "README-WEBSITE-SETUP.txt",
    "START_WEBSITE_NOW.txt",
    "WEBSITE-INSTALLATION-GUIDE.txt"
)

foreach ($f in $websiteFiles) {
    if (Test-Path "$historyPath\$f") {
        Write-Host "Targeting: $f -> Phase 11 (Website)"
        Move-Item -Path "$historyPath\$f" -Destination $phase11Dir -Force
    }
}

# 4. Meta Files: Move Roadmap and Checklist to Docs Root
$metaFiles = @("ROADMAP_V3_REBUILD.md", "PHASE_CHECKLIST_SUMMARY.md")
foreach ($f in $metaFiles) {
    if (Test-Path "$inProcessPath\$f") {
        Write-Host "Promoting: $f -> docs root"
        Move-Item -Path "$inProcessPath\$f" -Destination $docsPath -Force
    }
}

Write-Host "--- Ready! You can now run this script manually. ---" -ForegroundColor Green
