<# :
@echo off
setlocal
:: =====================================================================
:: GFC One-Click Deployer (v2.5) - Polyglot Version
:: =====================================================================

:: Admin Check
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo.
    echo [CRITICAL ERROR] This must be run as ADMINISTRATOR.
    echo Please right-click this file and choose "Run as Administrator".
    echo.
    pause
    exit /b
)

echo Starting GFC Deployment Engine...
echo.

:: Execute this same file as PowerShell
powershell -NoProfile -ExecutionPolicy Bypass -Command "iex ((Get-Content -Raw -LiteralPath '%~f0') -join \"`n\")"

echo.
echo Deployment Process Finished.
pause
exit /b
#>

# PowerShell Logic Starts Here
$zipName = "PublishGFCWebApp.zip"
$siteName = "GFCWebApp"
$appPool = "GFCWebApp"
$livePath = "C:\inetpub\GFCWebApp"
$stagingPath = "C:\inetpub\PublishGFCWebApp"
$backupRoot = "C:\inetpub\history"

function Write-Step($msg, $color = "Cyan") {
    Write-Host ""
    Write-Host ">>> $msg" -ForegroundColor $color
}

try {
    # 1. Locate the Zip
    Write-Step "Searching for $zipName..."
    $possiblePlaces = @(
        (Join-Path $env:USERPROFILE "Desktop"),
        "C:\Users\Lovanow\Desktop",
        "C:\Users\hnsil\Desktop",
        "."
    )
    
    $zipPath = $null
    foreach ($p in $possiblePlaces) {
        if (Test-Path $p) {
            $test = Join-Path $p $zipName
            if (Test-Path $test) { $zipPath = $test; break }
        }
    }

    if (-not $zipPath) {
        Write-Host "[INFO] No $zipName found. Putting things in order..." -ForegroundColor Yellow
        return
    }
    Write-Host "Found source: $zipPath" -ForegroundColor Green

    # 2. Cleanup & Staging
    Write-Step "Preparing staging area..."
    if (Test-Path $stagingPath) { Remove-Item $stagingPath -Recurse -Force -ErrorAction SilentlyContinue }
    New-Item -ItemType Directory -Path $stagingPath -Force | Out-Null

    # 3. Unzip
    Write-Step "Unzipping files..."
    Expand-Archive -Path $zipPath -DestinationPath $stagingPath -Force

    # 4. IIS Control
    Write-Step "Stopping IIS ($siteName)..."
    Import-Module WebAdministration -ErrorAction SilentlyContinue
    try { Stop-Website $siteName -ErrorAction SilentlyContinue } catch {}
    try { Stop-WebAppPool $appPool -ErrorAction SilentlyContinue } catch {}
    Start-Sleep -Seconds 2

    # 5. Backup with Robocopy (Reliable)
    Write-Step "Creating backup..."
    if (-not (Test-Path $backupRoot)) { New-Item -ItemType Directory -Path $backupRoot -Force | Out-Null }
    $ts = Get-Date -Format "yyyyMMdd_HHmmss"
    $backupPath = Join-Path $backupRoot "Backup_$ts"
    
    if (Test-Path $livePath) {
        # /E (subdirs), /COPYALL, /R:3 (retry), /W:1 (wait)
        robocopy $livePath $backupPath /E /R:3 /W:1 /NJH /NJS /NFL /NDL | Out-Null
        Write-Host "Backup saved: $backupPath" -ForegroundColor Gray
    }

    # 6. Deploy with Robocopy (Bypasses "leaf item" errors)
    Write-Step "Deploying files to live folder..."
    # /E (subdirs), /PURGE (to remove old files not in publish), /XF (exclude files)
    robocopy $stagingPath $livePath /E /XF "appsettings.Production.json" /R:8 /W:1 /NJH /NJS 
    
    # 7. Start IIS
    Write-Step "Starting IIS..." "Green"
    try { Start-WebAppPool $appPool -ErrorAction Stop } catch { Write-Host "Pool start error: $_" }
    try { Start-Website $siteName -ErrorAction Stop } catch { Write-Host "Site start error: $_" }

    # 8. Cleanup Source
    Write-Step "Cleaning up Desktop..."
    Remove-Item $zipPath -Force -ErrorAction SilentlyContinue
    Remove-Item $stagingPath -Recurse -Force -ErrorAction SilentlyContinue

    Write-Host ""
    Write-Host "=========================================" -ForegroundColor Green
    Write-Host "       DEPLOYMENT SUCCESSFUL!" -ForegroundColor Green
    Write-Host "=========================================" -ForegroundColor Green

} catch {
    Write-Host ""
    Write-Host "!!! FAILED !!!" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Yellow
}
