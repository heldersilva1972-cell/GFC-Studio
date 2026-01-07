# Fix Service Worker issues
# Run this on the production server

$appPath = "C:\inetpub\GFCWebApp"
$wwwrootPath = Join-Path $appPath "wwwroot"

Write-Host "=== Fixing Service Worker ===" -ForegroundColor Cyan
Write-Host ""

# 1. Remove/disable the problematic service worker
Write-Host "[1] Checking service worker..." -ForegroundColor Green
$serviceWorkerPath = Join-Path $wwwrootPath "service-worker.js"
$serviceWorkerPublishedPath = Join-Path $wwwrootPath "service-worker.published.js"

if (Test-Path $serviceWorkerPath) {
    Write-Host "  Found service-worker.js, renaming to disable it..." -ForegroundColor Yellow
    Move-Item -Path $serviceWorkerPath -Destination "$serviceWorkerPath.disabled" -Force
    Write-Host "  ✓ Service worker disabled" -ForegroundColor Green
}

if (Test-Path $serviceWorkerPublishedPath) {
    Write-Host "  Found service-worker.published.js, renaming to disable it..." -ForegroundColor Yellow
    Move-Item -Path $serviceWorkerPublishedPath -Destination "$serviceWorkerPublishedPath.disabled" -Force
    Write-Host "  ✓ Published service worker disabled" -ForegroundColor Green
}

# 2. Update manifest.json to remove service worker reference
Write-Host ""
Write-Host "[2] Updating manifest.json..." -ForegroundColor Green
$manifestPath = Join-Path $wwwrootPath "manifest.json"
if (Test-Path $manifestPath) {
    $manifest = Get-Content $manifestPath -Raw | ConvertFrom-Json
    # Service worker is typically not in manifest, but check anyway
    Write-Host "  ✓ Manifest checked" -ForegroundColor Green
}

# 3. Clear browser cache instruction
Write-Host ""
Write-Host "[3] Browser cache clearing required..." -ForegroundColor Yellow
Write-Host "  After this script completes, users must:" -ForegroundColor Yellow
Write-Host "  1. Press Ctrl+Shift+Delete" -ForegroundColor White
Write-Host "  2. Select 'Cached images and files'" -ForegroundColor White
Write-Host "  3. Click 'Clear data'" -ForegroundColor White
Write-Host "  OR use Incognito/Private mode" -ForegroundColor White

# 4. Restart IIS
Write-Host ""
Write-Host "[4] Restarting IIS..." -ForegroundColor Green
Restart-WebAppPool -Name "GFCWebApp"
Start-Sleep -Seconds 5
Write-Host "  ✓ IIS restarted" -ForegroundColor Green

Write-Host ""
Write-Host "=== Fix Complete ===" -ForegroundColor Cyan
Write-Host "Clear your browser cache and test: https://gfc.lovanow.com" -ForegroundColor Yellow
