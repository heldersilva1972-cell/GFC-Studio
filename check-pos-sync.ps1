# GFC POS & Server Revision Integrity Diagnostics
# Diagnoses the exact versioning state of the POS app, Local Server settings, and Remote Server/DB.

$ErrorActionPreference = "SilentlyContinue"
Clear-Host

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "     GFC POS & SERVER REVISION INTEGRITY DIAGNOSTIC" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host ""

$basePath = $PSScriptRoot
$apiUrl = "https://gfc.lovanow.com"

# --- LOCAL PATHS ---
$posProps = Join-Path $basePath "apps/GFC-Pos-Standalone/PosVersion.props"
$posVersionService = Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.UI/Services/PosVersionService.cs"
$posCsproj = Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.Mobile/GFC.Pos.Mobile.csproj"
$webappSettings = Join-Path $basePath "apps/webapp/GFC.BlazorServer/appsettings.json"

$posTerminalVersionTxt = Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.Terminal/wwwroot/version.txt"
$posUiVersionTxt = Join-Path $basePath "apps/GFC-Pos-Standalone/GFC.Pos.UI/wwwroot/version.txt"
$serverVersionTxt = Join-Path $basePath "apps/webapp/GFC.BlazorServer/wwwroot/version.txt"

# --- READ LOCAL VERSIONS ---
Write-Host "[*] Reading Local POS App Configuration..." -ForegroundColor Yellow

$localPropsVersion = "Not Found"
$localPropsBuild = "Not Found"
if (Test-Path $posProps) {
    $content = Get-Content $posProps -Raw
    if ($content -match '<PosVersion>(.*)</PosVersion>') { $localPropsVersion = $Matches[1].Trim() }
    if ($content -match '<PosBuild>(.*)</PosBuild>') { $localPropsBuild = $Matches[1].Trim() }
    Write-Host "    [OK] PosVersion.props: $localPropsVersion (Build: $localPropsBuild)" -ForegroundColor Green
} else {
    Write-Host "    [ERROR] PosVersion.props NOT FOUND at $posProps" -ForegroundColor Red
}

$localServiceVersion = "Not Found"
if (Test-Path $posVersionService) {
    $content = Get-Content $posVersionService -Raw
    if ($content -match 'return\s*"([^"]+)"') { $localServiceVersion = $Matches[1].Trim() }
    elseif ($content -match 'GetRevision\(\)\s*=>\s*"([^"]+)"') { $localServiceVersion = $Matches[1].Trim() }
    Write-Host "    [OK] PosVersionService.cs: $localServiceVersion" -ForegroundColor Green
} else {
    Write-Host "    [ERROR] PosVersionService.cs NOT FOUND at $posVersionService" -ForegroundColor Red
}

$localCsprojVersion = "Not Found"
$localCsprojBuild = "Not Found"
if (Test-Path $posCsproj) {
    $content = Get-Content $posCsproj -Raw
    if ($content -match '<ApplicationDisplayVersion>(.*)</ApplicationDisplayVersion>') { $localCsprojVersion = $Matches[1].Trim() }
    if ($content -match '<ApplicationVersion>(.*)</ApplicationVersion>') { $localCsprojBuild = $Matches[1].Trim() }
    Write-Host "    [OK] GFC.Pos.Mobile.csproj: DisplayVersion=$localCsprojVersion, Version=$localCsprojBuild" -ForegroundColor Green
} else {
    Write-Host "    [WARNING] GFC.Pos.Mobile.csproj NOT FOUND at $posCsproj" -ForegroundColor Yellow
}

$localTerminalTxt = "Not Found"
if (Test-Path $posTerminalVersionTxt) { $localTerminalTxt = (Get-Content $posTerminalVersionTxt).Trim() }
$localUiTxt = "Not Found"
if (Test-Path $posUiVersionTxt) { $localUiTxt = (Get-Content $posUiVersionTxt).Trim() }
$localServerTxt = "Not Found"
if (Test-Path $serverVersionTxt) { $localServerTxt = (Get-Content $serverVersionTxt).Trim() }

Write-Host "    [OK] Terminal version.txt: $localTerminalTxt" -ForegroundColor Green
Write-Host "    [OK] UI version.txt: $localUiTxt" -ForegroundColor Green
Write-Host "    [OK] BlazorServer version.txt: $localServerTxt" -ForegroundColor Green

Write-Host ""
Write-Host "[*] Reading Local Server Configuration (appsettings.json)..." -ForegroundColor Yellow
$localServerPosRevision = "Not Found"
$localServerPosBuild = "Not Found"
if (Test-Path $webappSettings) {
    $s = Get-Content $webappSettings | ConvertFrom-Json
    $localServerPosRevision = $s.ApplicationVersion.PosRevision
    $localServerPosBuild = $s.ApplicationVersion.PosBuildNumber
    Write-Host "    [OK] appsettings.json PosRevision: $localServerPosRevision (Build: $localServerPosBuild)" -ForegroundColor Green
} else {
    Write-Host "    [ERROR] appsettings.json NOT FOUND at $webappSettings" -ForegroundColor Red
}

# --- READ REMOTE SERVER & DB STATE ---
Write-Host ""
Write-Host "[*] Querying Remote Server ($apiUrl)..." -ForegroundColor Yellow
$timestamp = [DateTime]::UtcNow.Ticks

# Bypass SSL errors and use curl.exe for reliability over Cloudflare TLS
$remoteDbRevision = "Error connecting/parsing"
$posApiUrl = "$apiUrl/api/mobile-reporting/pos-version?t=$timestamp"
try {
    $response = & curl.exe -s -k $posApiUrl
    if ($response) {
        $remoteDbRevision = ($response | Out-String).Trim()
        Write-Host "    [OK] Remote DB Revision: $remoteDbRevision" -ForegroundColor Green
    } else {
        Write-Host "    [ERROR] Empty response from DB POS Version endpoint." -ForegroundColor Red
    }
} catch {
    Write-Host "    [ERROR] Failed to fetch DB POS Version from $posApiUrl. Msg: $_" -ForegroundColor Red
}

$remoteTxtRevision = "Error connecting"
$remoteTxtUrl = "$apiUrl/version.txt?v=$timestamp"
try {
    $response = & curl.exe -s -k $remoteTxtUrl
    if ($response) {
        $remoteTxtRevision = ($response | Out-String).Trim()
        Write-Host "    [OK] Remote version.txt: $remoteTxtRevision" -ForegroundColor Green
    } else {
        Write-Host "    [ERROR] Empty response from version.txt endpoint." -ForegroundColor Red
    }
} catch {
    Write-Host "    [ERROR] Failed to fetch version.txt from $remoteTxtUrl. Msg: $_" -ForegroundColor Red
}

# --- COMPARATIVE DIAGNOSTIC ANALYSIS ---
Write-Host ""
Write-Host "==================== DIAGNOSTIC REPORT ====================" -ForegroundColor Cyan
Write-Host ""

$mismatches = 0

# Check Local Sync State
if ($localPropsVersion -ne $localServiceVersion -or $localPropsVersion -ne $localTerminalTxt -or $localPropsVersion -ne $localUiTxt) {
    $mismatches++
    Write-Host "[FAIL] LOCAL DESKTOP FILES ARE OUT OF SYNC!" -ForegroundColor Red
    Write-Host "       - Props Version:    $localPropsVersion" -ForegroundColor White
    Write-Host "       - Service Version:  $localServiceVersion" -ForegroundColor White
    Write-Host "       - Terminal txt:     $localTerminalTxt" -ForegroundColor White
    Write-Host "       - UI txt:           $localUiTxt" -ForegroundColor White
    Write-Host "       >> SUGGESTION: Run `.\sync-version.ps1 -Project POS -Version <ver>` to force a re-sync." -ForegroundColor Yellow
} else {
    Write-Host "[PASS] Local POS code files are fully synchronized at version: $localPropsVersion" -ForegroundColor Green
}

# Check Local Server Settings vs POS files
if ($localServerPosRevision -ne $localPropsVersion) {
    $mismatches++
    Write-Host "[FAIL] LOCAL BLAZOR SERVER SETTINGS MISMATCH POS VERSION!" -ForegroundColor Red
    Write-Host "       - POS Code Files: $localPropsVersion" -ForegroundColor White
    Write-Host "       - appsettings.json: $localServerPosRevision" -ForegroundColor White
    Write-Host "       >> SUGGESTION: Run `.\sync-version.ps1 -Project POS -Version $localPropsVersion` again to update appsettings.json." -ForegroundColor Yellow
} else {
    Write-Host "[PASS] Local Blazor appsettings.json matches POS code files: $localServerPosRevision" -ForegroundColor Green
}

# Check Local Server vs Remote Server
if ($remoteDbRevision -notlike "*$localServerPosRevision*") {
    $mismatches++
    Write-Host "[FAIL] REMOTE DATABASE REVISION DOES NOT MATCH LOCAL REVISION!" -ForegroundColor Red
    Write-Host "       - Local Target: $localServerPosRevision" -ForegroundColor White
    Write-Host "       - Remote DB:    $remoteDbRevision" -ForegroundColor White
    Write-Host "       >> SUGGESTION 1: The remote database sync API did not succeed or has not run." -ForegroundColor Yellow
    Write-Host "          You can manually trigger a sync by executing this command in PowerShell:" -ForegroundColor Yellow
    Write-Host "          Invoke-RestMethod -Uri '$apiUrl/api/mobile-reporting/sync-version?project=POS&version=$localServerPosRevision&apiKey=GFC_SYNC_V2_SECRET_2026' -Method Post -Body '' -ContentType 'application/json'" -ForegroundColor Cyan
    Write-Host "       >> SUGGESTION 2: If suggestions above don't work, deploy the server to sync the Database." -ForegroundColor Yellow
} else {
    Write-Host "[PASS] Remote DB revision matches local configuration ($localServerPosRevision)" -ForegroundColor Green
}

# Check Remote version.txt vs Target version
if ($remoteTxtRevision -ne $localServerPosRevision) {
    $mismatches++
    Write-Host "[FAIL] REMOTE STATIC FILES (version.txt) DO NOT MATCH TARGET VERSION!" -ForegroundColor Red
    Write-Host "       - Remote version.txt: $remoteTxtRevision" -ForegroundColor White
    Write-Host "       - Target Version:     $localServerPosRevision" -ForegroundColor White
    Write-Host "       >> SUGGESTION: The WebApp files have not been successfully deployed/published to the IIS server." -ForegroundColor Yellow
    Write-Host "          Publish and copy the WebApp files using Deploy_WebApp.bat / Publish_WebApp.bat." -ForegroundColor Yellow
} else {
    Write-Host "[PASS] Remote static version.txt matches target version ($localServerPosRevision)" -ForegroundColor Green
}

Write-Host ""
if ($mismatches -eq 0) {
    Write-Host "SUCCESS: No discrepancy found between local configuration, files, and server database!" -ForegroundColor Green
} else {
    Write-Host "WARNING: Found $mismatches alignment issues. Follow the suggestions above to resolve." -ForegroundColor Red
}

Write-Host ""
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host " DIAGNOSTIC COMPLETE" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
