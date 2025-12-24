$baseDir = "docs\in-process"

# Define the unified structure mapping
$mapping = @{
    "PHASE_6A_STUDIO"    = "PHASE_6A_STUDIO_SOFTWARE_BUILD.md"
    "PHASE_6B_WEBSITE"   = "PHASE_6B_MODERNIZED_WEBSITE_LAUNCH.md"
    "PHASE_7_MOBILE"     = "PHASE_7_MOBILE_APP_INTEGRATION.md"
    "PHASE_8_AUTOMATION" = "PHASE_8_ADVANCED_AUTOMATION.md"
    "PHASE_9_POLISH"     = "PHASE_9_FINAL_POLISH.md"
}

# Create folders and move files
foreach ($key in $mapping.Keys) {
    $folderPath = Join-Path $baseDir $key
    if (-not (Test-Path $folderPath)) {
        Write-Host "Creating folder: $folderPath"
        New-Item -ItemType Directory -Path $folderPath | Out-Null
    }

    $sourceFile = Join-Path $baseDir $mapping[$key]
    $destFile = Join-Path $folderPath "MASTER_PLAN.md"

    if (Test-Path $sourceFile) {
        Write-Host "Moving $sourceFile to $destFile"
        Move-Item -Path $sourceFile -Destination $destFile -Force
    }
}

# Delete legacy/duplicate files
$filesToDelete = @(
    "PHASE_6_ECOSYSTEM_MERGER.md",
    "PHASE_6_WEBSITE_MODERNIZATION_STRATEGY.md",
    "REMAINING_PHASES_ROADMAP.md",
    "STUDIO_REMAINING_IMPLEMENTATION.md"
)

foreach ($file in $filesToDelete) {
    $filePath = Join-Path $baseDir $file
    if (Test-Path $filePath) {
        Write-Host "Deleting $filePath"
        Remove-Item -Path $filePath -Force
    }
}

Write-Host "Organization complete."
