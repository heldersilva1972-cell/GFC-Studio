# ============================================================================
# Move Phase 9 to Complete Folder
# ============================================================================
# Purpose: Move Phase 9 documentation from in-process to complete
# Date: 2025-12-24
# Reason: Phase 9 has been fully installed and is now complete
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Moving Phase 9 to Complete" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$baseDir = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\docs"
$inProcessDir = "$baseDir\in-process"
$completeDir = "$baseDir\complete"

# ============================================================================
# Step 1: Move Phase 9 folder to complete
# ============================================================================
Write-Host "[1/3] Moving Phase 9 folder..." -ForegroundColor Yellow

$phase9Source = "$inProcessDir\PHASE_9_POLISH"
$phase9Dest = "$completeDir\PHASE_9_POLISH"

if (Test-Path $phase9Source) {
    # Check if destination already exists
    if (Test-Path $phase9Dest) {
        Write-Host "  ⚠ Destination already exists. Removing old version..." -ForegroundColor Yellow
        Remove-Item -Path $phase9Dest -Recurse -Force
    }
    
    # Move the folder
    Move-Item -Path $phase9Source -Destination $phase9Dest -Force
    Write-Host "  ✓ Moved PHASE_9_POLISH to complete folder" -ForegroundColor Green
    
    # Count files
    $fileCount = (Get-ChildItem -Path $phase9Dest -File -Recurse).Count
    Write-Host "  ✓ Moved $fileCount files" -ForegroundColor Green
} else {
    Write-Host "  ✗ Phase 9 folder not found in in-process" -ForegroundColor Red
    Write-Host "  Location checked: $phase9Source" -ForegroundColor Gray
}

# ============================================================================
# Step 2: Move Jules task to archive
# ============================================================================
Write-Host ""
Write-Host "[2/3] Archiving completed Jules task..." -ForegroundColor Yellow

$julesTask = "$inProcessDir\JULES_TASK_PHASE_6A_4_STAFF_SHIFTS_UX.md"
$archiveJulesDir = "$baseDir\archive\jules_tasks"

if (Test-Path $julesTask) {
    # Create archive directory if it doesn't exist
    if (-not (Test-Path $archiveJulesDir)) {
        New-Item -ItemType Directory -Path $archiveJulesDir -Force | Out-Null
        Write-Host "  ✓ Created archive directory: jules_tasks" -ForegroundColor Green
    }
    
    # Move the file
    Move-Item -Path $julesTask -Destination "$archiveJulesDir\JULES_TASK_PHASE_6A_4_STAFF_SHIFTS_UX.md" -Force
    Write-Host "  ✓ Moved Jules task to archive" -ForegroundColor Green
} else {
    Write-Host "  ℹ Jules task file not found (may already be moved)" -ForegroundColor Gray
}

# ============================================================================
# Step 3: Display Summary
# ============================================================================
Write-Host ""
Write-Host "[3/3] Summary" -ForegroundColor Yellow
Write-Host ""

Write-Host "Completed Phases:" -ForegroundColor Cyan
Get-ChildItem -Path $completeDir -Directory | Where-Object { $_.Name -like "PHASE_*" } | Sort-Object Name | ForEach-Object {
    $fileCount = (Get-ChildItem -Path $_.FullName -File -Recurse).Count
    Write-Host "  ✓ $($_.Name) ($fileCount files)" -ForegroundColor Green
}

Write-Host ""
Write-Host "In-Process Phases:" -ForegroundColor Cyan
Get-ChildItem -Path $inProcessDir -Directory | Where-Object { $_.Name -like "PHASE_*" } | Sort-Object Name | ForEach-Object {
    $fileCount = (Get-ChildItem -Path $_.FullName -File -Recurse).Count
    Write-Host "  - $($_.Name) ($fileCount files)" -ForegroundColor Gray
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Phase 9 Successfully Moved to Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
