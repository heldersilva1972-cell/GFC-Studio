# GFC Studio Version Verification Script
# This script diagnoses the exact versioning state of the GFC IIS Web Server and Database

$ErrorActionPreference = "SilentlyContinue"
Clear-Host

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host " GFC STUDIO VERSION & IIS INTEGRITY DIAGNOSTIC" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host ""

# 1. Resolve configured folders and endpoints
$iisPath = "C:\inetpub\wwwroot"
$apiUrl = "https://gfc.lovanow.com"

# Allow user to specify custom path if needed
if ($args.Count -ge 1) { $iisPath = $args[0] }
if ($args.Count -ge 2) { $apiUrl = $args[1] }

Write-Host "[*] Checking Local IIS Directory: $iisPath" -ForegroundColor Yellow

# 2. Check physical version.txt file
$versionTxtFile = Join-Path $iisPath "version.txt"
if (Test-Path $versionTxtFile) {
    $txtContent = (Get-Content $versionTxtFile).Trim()
    Write-Host "[OK] version.txt exists." -ForegroundColor Green
    Write-Host "     Content: '$txtContent'" -ForegroundColor White
} else {
    Write-Host "[WARNING] version.txt NOT FOUND in $iisPath" -ForegroundColor Red
}

# 3. Check physical version.json file
$versionJsonFile = Join-Path $iisPath "version.json"
if (Test-Path $versionJsonFile) {
    $jsonContent = (Get-Content $versionJsonFile).Trim()
    Write-Host "[OK] version.json exists." -ForegroundColor Green
    Write-Host "     Content: '$jsonContent'" -ForegroundColor White
} else {
    Write-Host "[INFO] version.json not found (optional)." -ForegroundColor Gray
}

# 4. Check service-worker-assets.js for GFC revision cache track
$swAssetsFile = Join-Path $iisPath "service-worker-assets.js"
if (Test-Path $swAssetsFile) {
    $swContent = Get-Content $swAssetsFile -Tail 20 | Out-String
    # Extract version from assetsManifest line or comments
    Write-Host "[OK] service-worker-assets.js exists." -ForegroundColor Green
    if ($swContent -match "offline-cache-(2\.\d+\.\d+)") {
        Write-Host "     PWA Cache Tracked Version: '$($Matches[1])'" -ForegroundColor White
    } else {
        Write-Host "     Could not auto-parse offline-cache version key from sw-assets." -ForegroundColor Gray
    }
} else {
    Write-Host "[WARNING] service-worker-assets.js not found (Is this a Blazor Server instead of WASM?)" -ForegroundColor Gray
}

# 5. Check Live API Endpoint versions (Cache Busted)
Write-Host ""
Write-Host "[*] Querying Live HTTPS Endpoint: $apiUrl" -ForegroundColor Yellow
$timestamp = [DateTime]::UtcNow.Ticks

# Webapp API POS Version Check
$posApiUrl = "$apiUrl/api/mobile-reporting/pos-version?t=$timestamp"
try {
    $posApiResponse = Invoke-RestMethod -Uri $posApiUrl -Method Get -TimeoutSec 5
    Write-Host "[OK] API Endpoint /api/mobile-reporting/pos-version is reachable." -ForegroundColor Green
    Write-Host "     API Database POS Revision: '$($posApiResponse.Trim())'" -ForegroundColor White
} catch {
    Write-Host "[ERROR] Failed to query API Endpoint: $_" -ForegroundColor Red
}

# Physical version.txt check via Web Request
$webTxtUrl = "$apiUrl/version.txt?v=$timestamp"
try {
    $webTxtResponse = Invoke-RestMethod -Uri $webTxtUrl -Method Get -TimeoutSec 5
    Write-Host "[OK] Web Request /version.txt is reachable." -ForegroundColor Green
    Write-Host "     Web Version.txt Revision: '$($webTxtResponse.Trim())'" -ForegroundColor White
} catch {
    Write-Host "[ERROR] Failed to query Web /version.txt: $_" -ForegroundColor Red
}

Write-Host ""
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host " DIAGNOSTIC COMPLETE" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
