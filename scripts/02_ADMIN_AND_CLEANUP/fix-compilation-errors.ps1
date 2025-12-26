# GFC Studio V2 - Complete Compilation Errors Fix Script
# This script fixes all compilation errors after Jules' work

Write-Host "=== GFC Studio V2 - Complete Error Fix Script ===" -ForegroundColor Cyan
Write-Host ""

$projectRoot = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2"
Set-Location $projectRoot

Write-Host "Summary of fixes being applied:" -ForegroundColor Yellow
Write-Host "  1. Remove obsolete MudBlazor files" -ForegroundColor Gray
Write-Host "  2. Fix circular dependency (move VpnProfileRepository)" -ForegroundColor Gray
Write-Host "  3. Clean build artifacts" -ForegroundColor Gray
Write-Host "  4. Rebuild solution" -ForegroundColor Gray
Write-Host ""

# Step 1: Remove obsolete MudBlazor files
Write-Host "Step 1: Removing obsolete EnableSecureVideoAccess.razor files..." -ForegroundColor Yellow
$filesToRemove = @(
    "apps\webapp\GFC.BlazorServer\Components\Pages\Camera\EnableSecureVideoAccess.razor",
    "apps\webapp\GFC.BlazorServer\Components\Pages\Camera\EnableSecureVideoAccess.razor.css"
)

foreach ($file in $filesToRemove) {
    $fullPath = Join-Path $projectRoot $file
    if (Test-Path $fullPath) {
        Remove-Item $fullPath -Force
        Write-Host "  ✓ Removed: $file" -ForegroundColor Green
    } else {
        Write-Host "  ✓ Already removed: $file" -ForegroundColor Gray
    }
}

# Step 2: Fix circular dependency
Write-Host ""
Write-Host "Step 2: Fixing circular dependency..." -ForegroundColor Yellow

# Check if VpnProfileRepository is in wrong location
$wrongLocation = "apps\webapp\GFC.Data\Repositories\VpnProfileRepository.cs"
if (Test-Path (Join-Path $projectRoot $wrongLocation)) {
    Remove-Item (Join-Path $projectRoot $wrongLocation) -Force
    Write-Host "  ✓ Removed VpnProfileRepository from GFC.Data" -ForegroundColor Green
} else {
    Write-Host "  ✓ VpnProfileRepository already moved" -ForegroundColor Gray
}

# Verify it's in correct location
$correctLocation = "apps\webapp\GFC.BlazorServer\Repositories\VpnProfileRepository.cs"
if (Test-Path (Join-Path $projectRoot $correctLocation)) {
    Write-Host "  ✓ VpnProfileRepository in correct location (GFC.BlazorServer)" -ForegroundColor Green
} else {
    Write-Host "  ✗ VpnProfileRepository missing from GFC.BlazorServer" -ForegroundColor Red
}

# Step 3: Clean obj and bin folders
Write-Host ""
Write-Host "Step 3: Cleaning build artifacts..." -ForegroundColor Yellow

$foldersToClean = @(
    "apps\webapp\GFC.BlazorServer\obj",
    "apps\webapp\GFC.BlazorServer\bin",
    "apps\webapp\GFC.Core\obj",
    "apps\webapp\GFC.Core\bin",
    "apps\webapp\GFC.Data\obj",
    "apps\webapp\GFC.Data\bin"
)

foreach ($folder in $foldersToClean) {
    $fullPath = Join-Path $projectRoot $folder
    if (Test-Path $fullPath) {
        Remove-Item $fullPath -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "  ✓ Cleaned: $folder" -ForegroundColor Green
    }
}

# Step 4: Build the solution
Write-Host ""
Write-Host "Step 4: Building the solution..." -ForegroundColor Yellow
Write-Host "  Running: dotnet build apps\webapp\GFC.BlazorServer\GFC.BlazorServer.csproj" -ForegroundColor Gray
Write-Host ""

$buildOutput = dotnet build "apps\webapp\GFC.BlazorServer\GFC.BlazorServer.csproj" 2>&1

# Check build result
if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "╔════════════════════════════════╗" -ForegroundColor Green
    Write-Host "║   BUILD SUCCESSFUL ✓           ║" -ForegroundColor Green
    Write-Host "╚════════════════════════════════╝" -ForegroundColor Green
    Write-Host ""
    Write-Host "All compilation errors have been fixed!" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "╔════════════════════════════════╗" -ForegroundColor Red
    Write-Host "║   BUILD FAILED ✗               ║" -ForegroundColor Red
    Write-Host "╚════════════════════════════════╝" -ForegroundColor Red
    Write-Host ""
    Write-Host "Remaining errors:" -ForegroundColor Yellow
    $buildOutput | Select-String "error" | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
    Write-Host ""
}

Write-Host "Script completed." -ForegroundColor Cyan
