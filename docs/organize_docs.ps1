# GFC Documentation Organizer V3
# Run this from the 'docs' folder

$docsPath = Get-Location
$completePath = Join-Path $docsPath "complete"
$inProcessPath = Join-Path $docsPath "in-process"
$historyPath = Join-Path $docsPath "History_Logs"
$guidePath = Join-Path $docsPath "Guides"
$sqlPath = Join-Path $docsPath "DatabaseScripts"
$archivePath = Join-Path $docsPath "archive"
$masterPlanPath = Join-Path $docsPath "master-plans"

# Ensure core directories exist
@($completePath, $inProcessPath, $historyPath, $guidePath, $sqlPath, $archivePath, $masterPlanPath) | ForEach-Object {
    if (!(Test-Path $_)) { New-Item -ItemType Directory -Path $_ | Out-Null }
}

Write-Host "Organizing Phase Documents..." -ForegroundColor Cyan

# Define a function to process a folder
function Organize-Folder($sourcePath) {
    if (!(Test-Path $sourcePath)) { return }
    
    Get-ChildItem -Path $sourcePath -File -Include "*.md", "*.txt" | ForEach-Object {
        $file = $_
        $fileName = $file.Name.ToUpper()
        $targetDir = $null

        # 1. Master Plans
        if ($fileName -like "*MASTER_PLAN*") {
            $targetDir = $masterPlanPath
        }
        # 2. Completed Phases (1-6, 10, or contains COMPLETE/SUMMARY)
        elseif ($fileName -match "PHASE_([0-6]|10)" -or $fileName -like "*COMPLETE*" -or $fileName -like "*SUMMARY*") {
            $targetDir = $completePath
        }
        # 3. In-Process Phases (7-9, 11-16)
        elseif ($fileName -match "PHASE_(7|8|9|11|12|13|14|15|16)" -or $fileName -match "PHASE_CAMERA") {
            $targetDir = $inProcessPath
        }
        # 4. Topic Mapping
        elseif ($fileName -like "*BARTENDER*" -or $fileName -like "*SHIFT*" -or $fileName -like "*STAFF*") {
            $targetDir = Join-Path $inProcessPath "PHASE_7_MOBILE"
        }
        elseif ($fileName -like "*RENTAL*" -or $fileName -like "*WEBSITE*" -or $fileName -like "*BOOKING*") {
            $targetDir = Join-Path $inProcessPath "PHASE_11_NEW_GFC_WEBSITE"
        }
        elseif ($fileName -like "*WIREGUARD*" -or $fileName -like "*CLOUDFLARE*" -or $fileName -like "*REMOTE*ACCESS*") {
            $targetDir = Join-Path $inProcessPath "PHASE_CAMERA_REMOTE_SECURITY"
        }

        if ($targetDir) {
            if (!(Test-Path $targetDir)) { New-Item -ItemType Directory -Path $targetDir | Out-Null }
            Move-Item -Path $file.FullName -Destination $targetDir -Force -ErrorAction SilentlyContinue
        }
    }
}

# Process each source
@($docsPath, $historyPath, $guidePath) | ForEach-Object { Organize-Folder $_ }

# Move SQL/PS1
Get-ChildItem -Path $docsPath -Filter "*.sql" | Move-Item -Destination $sqlPath -Force -ErrorAction SilentlyContinue
Get-ChildItem -Path $docsPath -Filter "*.ps1" | Where-Object { $_.Name -notmatch "organize_docs.ps1" } | Move-Item -Destination $archivePath -Force -ErrorAction SilentlyContinue

Write-Host "Done!" -ForegroundColor Green
