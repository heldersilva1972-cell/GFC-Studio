$filesToMove = @(
    "docs\PHASE_0_GFC_WEBSITE_IMPLEMENTATION_PLAN.md",
    "docs\PHASE_0_GFC_WEBSITE_SUMMARY.md",
    "docs\PHASE_1_DIAGNOSTICS_FOUNDATION.md",
    "docs\PHASE_2_DIAGNOSTICS_PERFORMANCE.md",
    "docs\PHASE_3_DIAGNOSTICS_HARDWARE_CAMERAS.md",
    "docs\PHASE_4_CAMERA_ENHANCEMENTS_ISSUE.md",
    "docs\PHASE_4_DIAGNOSTICS_HISTORY_ALERTS.md",
    "docs\PHASE_5_CAMERA_COMPLETION_ISSUE.md",
    "docs\PHASE_5_COMPLETION_POLISH.md",
    "docs\JULES_TASK_PHASE_5.md",
    "docs\DIAGNOSTICS_MODERNIZATION_COMPLETE.md",
    "docs\SYSTEM_DIAGNOSTICS_MODERNIZATION_PLAN.md",
    "docs\in-process\CAMERA_PHASE_1_CONNECTIVITY_PROOF_PLAN.md",
    "docs\in-process\CAMERA_PHASE_2_MODERN_UI_PLAN.md",
    "docs\in-process\CAMERA_PHASE_3_PLAYBACK_TIMELINE_PLAN.md",
    "docs\in-process\CAMERA_PHASE_4_AUDIT_ARCHIVE_PLAN.md",
    "docs\in-process\CAMERA_PHASE_5_MANAGEMENT_SECURITY_PLAN.md",
    "docs\in-process\PHASE_2_VISUAL_BUILDER_PLAN.md",
    "docs\in-process\PHASE_3_AUTO_OPEN_PLAN.md",
    "docs\in-process\PHASE_4_ADVANCED_MODES_PLAN.md",
    "docs\in-process\PHASE_5_CLEANUP_AND_POLISH_PLAN.md",
    "docs\in-process\PURGE_SIMULATION_MODE_PLAN.md"
)

$dest = "docs\complete"

if (!(Test-Path $dest)) {
    New-Item -ItemType Directory -Path $dest | Out-Null
}

foreach ($file in $filesToMove) {
    if (Test-Path $file) {
        Write-Host "Moving $file..."
        Move-Item -Path $file -Destination $dest -Force
    } else {
        Write-Host "File not found: $file"
    }
}
Write-Host "Move operation completed."
