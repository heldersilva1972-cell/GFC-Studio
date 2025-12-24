# MoveCompletedDocs.ps1
# Move all documents for completed Phase 6A tasks and related plans

$SourceBase = "docs\in-process"
$DestBase = "docs\complete"

# Ensure destination exists
if (!(Test-Path $DestBase)) {
    New-Item -ItemType Directory -Path $DestBase | Out-Null
}

# List of specific files to move with their target names
$FilesToMove = @(
    @{ Path = "PHASE_6A_STUDIO\MASTER_PLAN.md"; NewName = "PHASE_6A_MASTER_PLAN_COMPLETE.md" },
    @{ Path = "PHASE_6A_STUDIO\PHASE_6A_1_STUDIO_CORE.md"; NewName = "PHASE_6A_1_STUDIO_CORE_COMPLETE.md" },
    @{ Path = "PHASE_6A_STUDIO\PHASE_6A_2_ADVANCED_TOOLS.md"; NewName = "PHASE_6A_2_ADVANCED_TOOLS_COMPLETE.md" },
    @{ Path = "PHASE_6A_STUDIO\PHASE_6A_3_MISSION_CONTROL.md"; NewName = "PHASE_6A_3_MISSION_CONTROL_COMPLETE.md" },
    @{ Path = "PHASE_6A_STUDIO\JULES_HANDOVER_PHASE_6A_2.md"; NewName = "PHASE_6A_2_HANDOVER_COMPLETE.md" },
    @{ Path = "JULES_TASK_PHASE_6A_3_MISSION_CONTROL.md"; NewName = "JULES_TASK_PHASE_6A_3_COMPLETE.md" },
    @{ Path = "NOTIFICATION_FINAL_IMPLEMENTATION_PLAN.md"; NewName = "NOTIFICATION_FINAL_IMPLEMENTATION_PLAN_COMPLETE.md" }
)

foreach ($item in $FilesToMove) {
    $src = Join-Path $SourceBase $item.Path
    $dst = Join-Path $DestBase $item.NewName
    
    if (Test-Path $src) {
        Write-Host "Moving: $($item.Path) -> $($item.NewName)"
        Move-Item -Path $src -Destination $dst -Force
    } else {
        Write-Host "Skipping (not found): $src" -ForegroundColor Gray
    }
}

# Cleanup empty Studio folder if all sub-files were moved
$studioFolder = Join-Path $SourceBase "PHASE_6A_STUDIO"
if (Test-Path $studioFolder) {
    if ((Get-ChildItem $studioFolder).Count -eq 0) {
        Remove-Item $studioFolder
        Write-Host "Removed empty directory: $studioFolder"
    }
}

Write-Host "`nSync Complete. Documents moved to $DestBase" -ForegroundColor Green
