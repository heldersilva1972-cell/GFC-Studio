# ============================================================================
# Phase 9 Documentation Organization Script
# ============================================================================
# Purpose: Move Jules task file to appropriate phase folder
# Date: 2025-12-24
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Phase 9 Documentation Organization" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$baseDir = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\docs"
$inProcessDir = "$baseDir\in-process"

# ============================================================================
# Step 1: Move Jules Task File to Archive (it's already completed)
# ============================================================================
Write-Host "[1/3] Moving completed Jules task to archive..." -ForegroundColor Yellow

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
# Step 2: Verify Phase 9 Structure
# ============================================================================
Write-Host ""
Write-Host "[2/3] Verifying Phase 9 structure..." -ForegroundColor Yellow

$phase9Dir = "$inProcessDir\PHASE_9_POLISH"
$phase9Files = @(
    "MASTER_PLAN.md",
    "PHASE_9_1_DATA_MIGRATION.md",
    "PHASE_9_2_PERFORMANCE_AND_DOCS.md"
)

$allPresent = $true
foreach ($file in $phase9Files) {
    $filePath = "$phase9Dir\$file"
    if (Test-Path $filePath) {
        Write-Host "  ✓ Found: $file" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Missing: $file" -ForegroundColor Red
        $allPresent = $false
    }
}

# ============================================================================
# Step 3: Display Summary
# ============================================================================
Write-Host ""
Write-Host "[3/3] Summary" -ForegroundColor Yellow
Write-Host "  Phase 9 Location: $phase9Dir" -ForegroundColor Cyan
Write-Host "  Files in Phase 9: $(($phase9Files).Count)" -ForegroundColor Cyan

if ($allPresent) {
    Write-Host ""
    Write-Host "✓ All Phase 9 files are correctly organized!" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "⚠ Some Phase 9 files are missing. Please check the installation." -ForegroundColor Yellow
}

# ============================================================================
# Step 4: List all Phase folders for reference
# ============================================================================
Write-Host ""
Write-Host "Current Phase Folders:" -ForegroundColor Cyan
Get-ChildItem -Path $inProcessDir -Directory | Where-Object { $_.Name -like "PHASE_*" } | ForEach-Object {
    $fileCount = (Get-ChildItem -Path $_.FullName -File).Count
    Write-Host "  - $($_.Name) ($fileCount files)" -ForegroundColor Gray
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Organization Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
