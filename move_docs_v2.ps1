$sourceDir = "docs\in-process"
$destDir = "docs\complete"

# Move Phase 6A Studio files
$studioFiles = Get-ChildItem -Path "$sourceDir\PHASE_6A_STUDIO" -Filter "*.md"
foreach ($file in $studioFiles) {
    if ($file.Name -eq "MASTER_PLAN.md") {
        Move-Item -Path $file.FullName -Destination "$destDir\PHASE_6A_MASTER_PLAN_COMPLETE.md" -Force
    } elseif ($file.Name -eq "JULES_HANDOVER_PHASE_6A_2.md") {
        Move-Item -Path $file.FullName -Destination "$destDir\JULES_HANDOVER_PHASE_6A_2_COMPLETE.md" -Force
    } else {
        $newName = $file.BaseName + "_COMPLETE.md"
        Move-Item -Path $file.FullName -Destination "$destDir\$newName" -Force
    }
}

# Move other confirmed complete files from in-process
$otherFiles = @(
    "JULES_TASK_PHASE_6A_3_MISSION_CONTROL.md",
    "NOTIFICATION_FINAL_IMPLEMENTATION_PLAN.md"
)

foreach ($fileName in $otherFiles) {
    $filePath = Join-Path $sourceDir $fileName
    if (Test-Path $filePath) {
        $baseName = [System.IO.Path]::GetFileNameWithoutExtension($fileName)
        Move-Item -Path $filePath -Destination "$destDir\$baseName`_COMPLETE.md" -Force
    }
}

Write-Host "Completed moving docs."
