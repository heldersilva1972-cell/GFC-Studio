# ============================================================================
# Move Phase 10 to Complete Folder
# ============================================================================
# Purpose: Move Phase 10 documentation from in-process to complete
# Date: 2025-12-24
# Reason: Phase 10 Foundation & Core Infrastructure is installed and complete
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Moving Phase 10 to Complete" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$baseDir = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\docs"
$inProcessDir = "$baseDir\in-process"
$completeDir = "$baseDir\complete"

# ============================================================================
# Step 1: Move Phase 10 folder to complete
# ============================================================================
Write-Host "[1/2] Moving Phase 10 folder..." -ForegroundColor Yellow

$phase10Source = "$inProcessDir\PHASE_10_STUDIO_V2_REBUILD"
$phase10Dest = "$completeDir\PHASE_10_STUDIO_V2_REBUILD"

if (Test-Path $phase10Source) {
    # Ensure complete directory exists
    if (-not (Test-Path $completeDir)) {
        New-Item -ItemType Directory -Path $completeDir -Force | Out-Null
        Write-Host "  ✓ Created complete directory" -ForegroundColor Green
    }

    # Check if destination already exists
    if (Test-Path $phase10Dest) {
        Write-Host "  ⚠ Destination already exists. Removing old version..." -ForegroundColor Yellow
        Remove-Item -Path $phase10Dest -Recurse -Force
    }
    
    # Move the folder
    Move-Item -Path $phase10Source -Destination $phase10Dest -Force
    Write-Host "  ✓ Moved PHASE_10_STUDIO_V2_REBUILD to complete folder" -ForegroundColor Green
    
    # Count files
    $fileCount = (Get-ChildItem -Path $phase10Dest -File -Recurse).Count
    Write-Host "  ✓ Moved $fileCount files" -ForegroundColor Green
} else {
    Write-Host "  ✗ Phase 10 folder not found in in-process" -ForegroundColor Red
    Write-Host "  Location checked: $phase10Source" -ForegroundColor Gray
}

# ============================================================================
# Step 2: Display Summary
# ============================================================================
Write-Host ""
Write-Host "[2/2] Summary" -ForegroundColor Yellow
Write-Host ""

Write-Host "Completed Phases:" -ForegroundColor Cyan
if (Test-Path $completeDir) {
    Get-ChildItem -Path $completeDir -Directory | Where-Object { $_.Name -like "PHASE_*" } | Sort-Object Name | ForEach-Object {
        $fileCount = (Get-ChildItem -Path $_.FullName -File -Recurse).Count
        Write-Host "  ✓ $($_.Name) ($fileCount files)" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "In-Process Phases:" -ForegroundColor Cyan
if (Test-Path $inProcessDir) {
    Get-ChildItem -Path $inProcessDir -Directory | Where-Object { $_.Name -like "PHASE_*" } | Sort-Object Name | ForEach-Object {
        $fileCount = (Get-ChildItem -Path $_.FullName -File -Recurse).Count
        Write-Host "  - $($_.Name) ($fileCount files)" -ForegroundColor Gray
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Phase 10 Successfully Moved to Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
