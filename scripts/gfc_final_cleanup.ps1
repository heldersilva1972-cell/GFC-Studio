# GFC Document & System Cleanup Script
# This script fixes the accidental root organization and deep-cleans the docs folder

$rootPath = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2"
$docsPath = "$rootPath\docs"
$inProcessPath = "$docsPath\in-process"
$historyPath = "$docsPath\History_Logs"
$sqlPath = "$docsPath\DatabaseScripts"
$archivePath = "$docsPath\archive"

Write-Host "--- Step 1: Cleaning up accidental root organization ---" -ForegroundColor Cyan

# Move root artifacts to docs
if (Test-Path "$rootPath\DatabaseScripts") {
    Get-ChildItem -Path "$rootPath\DatabaseScripts" -Filter "*.sql" | Move-Item -Destination "$sqlPath" -Force
    Remove-Item "$rootPath\DatabaseScripts" -Recurse -Force
}

if (Test-Path "$rootPath\archive") {
    Get-ChildItem -Path "$rootPath\archive" -Filter "*.ps1" | Move-Item -Destination "$archivePath" -Force
    Remove-Item "$rootPath\archive" -Recurse -Force
}

# Remove empty organizational folders from root
$rootFoldersToRemove = @("complete", "in-process", "master-plans", "Guides", "History_Logs")
foreach ($folder in $rootFoldersToRemove) {
    if (Test-Path "$rootPath\$folder") {
        Remove-Item "$rootPath\$folder" -Recurse -Force
    }
}

Write-Host "--- Step 2: Refining docs\in-process Accuracy ---" -ForegroundColor Cyan

# Define Phase 7 (Mobile/Staff) target
$phase7Path = "$inProcessPath\PHASE_7_MOBILE"
if (!(Test-Path $phase7Path)) { New-Item -ItemType Directory -Path $phase7Path | Out-Null }

# Move Phase 7 related files from History_Logs and In-Process Root
$phase7Files = @(
    "$historyPath\BARTENDER_SCHEDULE_ENHANCEMENTS.md",
    "$historyPath\STAFF_MANAGEMENT_IMPLEMENTATION_SUMMARY.md",
    "$inProcessPath\JULES_TASK_PHASE_6A_4_STAFF_SHIFTS_UX.md"
)

foreach ($file in $phase7Files) {
    if (Test-Path $file) {
        Write-Host "Moving $(Split-Path $file -Leaf) -> docs\in-process\PHASE_7_MOBILE"
        Move-Item -Path $file -Destination $phase7Path -Force
    }
}

# Move general "Website" setup docs to Phase 11
$phase11Path = "$inProcessPath\PHASE_11_NEW_GFC_WEBSITE"
$websiteDocs = @(
    "$historyPath\HOW_TO_START_WEBSITE.txt",
    "$historyPath\README-WEBSITE-SETUP.txt",
    "$historyPath\START_WEBSITE_NOW.txt",
    "$historyPath\WEBSITE-INSTALLATION-GUIDE.txt"
)

foreach ($file in $websiteDocs) {
    if (Test-Path $file) {
        Write-Host "Moving $(Split-Path $file -Leaf) -> docs\in-process\PHASE_11_NEW_GFC_WEBSITE"
        Move-Item -Path $file -Destination $phase11Path -Force
    }
}

# Move Simulation Mode to Phase 8 (Automation/Simulation is usually grouped)
$phase8Path = "$inProcessPath\PHASE_8_AUTOMATION"
if (Test-Path "$historyPath\Simulation_Mode_Engineering_Blueprint.txt") {
    Move-Item -Path "$historyPath\Simulation_Mode_Engineering_Blueprint.txt" -Destination $phase8Path -Force
}

Write-Host "Cleanup and Organization Complete!" -ForegroundColor Green
