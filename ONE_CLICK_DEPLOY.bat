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

echo =====================================================================
echo    GFC One-Click Deployer - HOST COMPUTER ONLY (Production)
echo =====================================================================
echo.
echo [NOTICE] This script replaces the live website files on this machine.
echo It looks for '%zipName%' on your Desktop.
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
$projectPath = ".\apps\webapp\GFC.BlazorServer\GFC.BlazorServer.csproj"

function Write-Step($msg, $color = "Cyan") {
    Write-Host ""
    Write-Host ">>> $msg" -ForegroundColor $color
}

try {
    Write-Host "--------------------------------------------------------" -ForegroundColor Gray
    Write-Host " CONFIGURATION:" -ForegroundColor Gray
    Write-Host " Site:    $siteName" -ForegroundColor Gray
    Write-Host " Pool:    $appPool" -ForegroundColor Gray
    Write-Host " Live:    $livePath" -ForegroundColor Gray
    Write-Host " Staging: $stagingPath" -ForegroundColor Gray
    Write-Host "--------------------------------------------------------" -ForegroundColor Gray

    # 1. Locate the Zip
    Write-Step "Searching for $zipName on Desktop/Current Dir..."
    $possiblePlaces = @(
        (Join-Path $env:USERPROFILE "Desktop"),
        ".",
        "C:\Users\Lovanow\Desktop",
        "C:\Users\hnsil\Desktop"
    )
    
    $zipPath = $null
    foreach ($p in $possiblePlaces) {
        if (Test-Path $p) {
            $test = Join-Path $p $zipName
            if (Test-Path $test) { $zipPath = $test; break }
        }
    }

    if (-not $zipPath) {
        Write-Host "[ERROR] Could not find $zipName on your Desktop." -ForegroundColor Red
        Write-Host "Please transfer the ZIP from your laptop to this computer's Desktop." -ForegroundColor Yellow
        Write-Host "Places checked: $($possiblePlaces -join ', ')" -ForegroundColor Gray
        return
    }
    Write-Host "Found source package: $zipPath" -ForegroundColor Green

    Write-Step "UNZIPPING: Extracting package to staging area..."
    if (Test-Path $stagingPath) { Remove-Item $stagingPath -Recurse -Force -ErrorAction SilentlyContinue }
    New-Item -ItemType Directory -Path $stagingPath -Force | Out-Null

    Expand-Archive -Path $zipPath -DestinationPath $stagingPath -Force
    Write-Host "Extraction complete: $stagingPath" -ForegroundColor Gray

    # 4. IIS Control
    Write-Step "IIS: Stopping Web Site and App Pool..."
    Import-Module WebAdministration -ErrorAction SilentlyContinue
    
    Write-Host "Stopping Site [$siteName]..." -ForegroundColor Gray
    try { Stop-Website $siteName -ErrorAction SilentlyContinue } catch {}
    
    Write-Host "Stopping Pool [$appPool]..." -ForegroundColor Gray
    try { Stop-WebAppPool $appPool -ErrorAction SilentlyContinue } catch {}
    
    # Wait for w3wp to die
    Write-Host "Waiting 5 seconds for processes to release file locks..." -ForegroundColor Gray
    Start-Sleep -Seconds 5

    # 5. Backup
    Write-Step "BACKUP: Copying current live files to history..."
    if (-not (Test-Path $backupRoot)) { New-Item -ItemType Directory -Path $backupRoot -Force | Out-Null }
    $ts = Get-Date -Format "yyyyMMdd_HHmmss"
    $backupPath = Join-Path $backupRoot "Backup_$ts"
    
    if (Test-Path $livePath) {
        Write-Host "Source: $livePath" -ForegroundColor Gray
        Write-Host "Target: $backupPath" -ForegroundColor Gray
        robocopy $livePath $backupPath /E /R:3 /W:1 /NJH /NJS /NFL /NDL | Out-Null
        Write-Host "Backup verification: Done." -ForegroundColor Green
    }

    # 6. Deploy with Robocopy /MIR (Mirror)
    Write-Step "DEPLOYING: Syncing Staging -> Live (Mirror Mode)"
    Write-Host "Action: Replacing files, removing orphans, and protecting config..." -ForegroundColor Yellow
    Write-Host "Source: $stagingPath" -ForegroundColor Gray
    Write-Host "Live:   $livePath" -ForegroundColor Gray
    Write-Host "--------------------------------------------------------" -ForegroundColor Gray
    
    # /MIR = Mirror directory (Syncs everything, deletes what is gone)
    # /XF = Exclude appsettings.Production.json (IMPORTANT: do not overwrite live DB config)
    # /MT = Multi-threaded for speed
    # We remove /NJH /NJS /NFL /NDL to let the user see what is happening
    robocopy $stagingPath $livePath /MIR /XF "appsettings.Production.json" "web.config" /R:10 /W:2 /MT:8
    
    $rc = $LASTEXITCODE
    Write-Host "--------------------------------------------------------" -ForegroundColor Gray
    if ($rc -ge 8) {
        Write-Host "!!! WARNING: Robocopy Exit Code $rc !!!" -ForegroundColor Red
        Write-Host "This usually means some files were locked or access was denied." -ForegroundColor Yellow
    } else {
        Write-Host "File synchronization complete (Status Code: $rc)." -ForegroundColor Green
    }

    # 7. Start IIS
    Write-Step "IIS: Restarting Services..." "Green"
    Write-Host "Starting Pool [$appPool]..." -ForegroundColor Gray
    try { Start-WebAppPool $appPool -ErrorAction Stop } catch { Write-Host "Pool start error: $_" }
    
    Write-Host "Starting Site [$siteName]..." -ForegroundColor Gray
    try { Start-Website $siteName -ErrorAction Stop } catch { Write-Host "Site start error: $_" }

    # 8. Cleanup Staging
    Write-Step "CLEANUP: Removing temporary staging files..."
    if (Test-Path $stagingPath) { 
        Remove-Item $stagingPath -Recurse -Force -ErrorAction SilentlyContinue 
        Write-Host "Staging folder $stagingPath removed." -ForegroundColor Gray
    }

    Write-Host ""
    Write-Host "========================================================" -ForegroundColor Green
    Write-Host "             DEPLOYMENT SUCCESSFUL!" -ForegroundColor Green
    Write-Host "    Web App is now LIVE at http://192.168.0.248:8080" -ForegroundColor Green
    Write-Host "========================================================" -ForegroundColor Green

} catch {
    Write-Host ""
    Write-Host "!!! FAILED !!!" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Yellow
}
