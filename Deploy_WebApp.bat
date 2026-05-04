@echo off
title GFC WebApp Deployer
echo.
echo ===================================================
echo   GFC WebApp Deployment Initializing...
echo ===================================================

net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [CRITICAL ERROR] This must be run as ADMINISTRATOR.
    pause
    exit /b
)

set "PS_PATH=%TEMP%\gfc_webapp_deploy.ps1"
if exist "%PS_PATH%" del "%PS_PATH%"

echo $zipName = "PublishGFCWebApp.zip" >> "%PS_PATH%"
echo $folders = @([Environment]::GetFolderPath("Desktop"), "C:\Users\Lovanow\Desktop", "C:\Users\hnsil\Desktop", ".") >> "%PS_PATH%"
echo function Write-Step($msg, $color = "Cyan") { Write-Host "" ; Write-Host ">>> $msg" -ForegroundColor $color } >> "%PS_PATH%"
echo try { >> "%PS_PATH%"
echo     $desktopPath = $null >> "%PS_PATH%"
echo     foreach ($f in $folders) { if ($f -and (Test-Path $f)) { $tp = Join-Path $f $zipName ; if (Test-Path $tp) { $desktopPath = $tp; break } } } >> "%PS_PATH%"
echo     if (-not $desktopPath) { Write-Host "[INFO] No $zipName found." -ForegroundColor Yellow ; Read-Host "Press Enter" ; return } >> "%PS_PATH%"
echo     $staging = "C:\inetpub\PublishGFCWebApp" >> "%PS_PATH%"
echo     $live = "C:\inetpub\GFCWebApp" >> "%PS_PATH%"
echo     $backupRoot = "C:\inetpub\history_webapp" >> "%PS_PATH%"
echo     Write-Step "Preparing staging..." >> "%PS_PATH%"
echo     if (Test-Path $staging) { Remove-Item $staging -Recurse -Force -ErrorAction SilentlyContinue } >> "%PS_PATH%"
echo     New-Item -ItemType Directory -Path $staging ^| Out-Null >> "%PS_PATH%"
echo     Write-Step "Unzipping..." >> "%PS_PATH%"
echo     Expand-Archive -Path $desktopPath -DestinationPath $staging -Force >> "%PS_PATH%"
echo     Write-Step "Stopping IIS (GFCWebApp)..." >> "%PS_PATH%"
echo     Import-Module WebAdministration -ErrorAction SilentlyContinue >> "%PS_PATH%"
echo     Stop-Website "GFCWebApp" -ErrorAction SilentlyContinue >> "%PS_PATH%"
echo     Stop-WebAppPool "GFCWebApp" -ErrorAction SilentlyContinue >> "%PS_PATH%"
echo     Start-Sleep -Seconds 2 >> "%PS_PATH%"
echo     Write-Step "Backing up site..." >> "%PS_PATH%"
echo     if (-not (Test-Path $backupRoot)) { New-Item -ItemType Directory -Path $backupRoot ^| Out-Null } >> "%PS_PATH%"
echo     $ts = Get-Date -Format "yyyyMMdd_HHmmss" >> "%PS_PATH%"
echo     $bp = Join-Path $backupRoot "Backup_$ts" >> "%PS_PATH%"
echo     if (Test-Path $live) { Copy-Item -Path "$live\*" -Destination $bp -Recurse -Force -ErrorAction SilentlyContinue } >> "%PS_PATH%"
echo     Write-Step "Deploying files..." >> "%PS_PATH%"
echo     $items = Get-ChildItem -Path $staging -Recurse >> "%PS_PATH%"
echo     foreach ($item in $items) { >> "%PS_PATH%"
echo         if ($item.Name -eq "appsettings.Production.json") { continue } >> "%PS_PATH%"
echo         $rel = $item.FullName.Substring($staging.Length).TrimStart("\") >> "%PS_PATH%"
echo         $dest = Join-Path $live $rel >> "%PS_PATH%"
echo         if ($item.PSIsContainer) { if (-not (Test-Path $dest)) { New-Item -ItemType Directory -Path $dest ^| Out-Null } } >> "%PS_PATH%"
echo         else { if (-not (Test-Path (Split-Path $dest))) { New-Item -ItemType Directory -Path (Split-Path $dest) ^| Out-Null } ; Copy-Item -Path $item.FullName -Destination $dest -Force } >> "%PS_PATH%"
echo     } >> "%PS_PATH%"
echo     Write-Step "Starting IIS..." "Green" >> "%PS_PATH%"
echo     Start-WebAppPool "GFCWebApp" -ErrorAction SilentlyContinue >> "%PS_PATH%"
echo     Start-Website "GFCWebApp" -ErrorAction SilentlyContinue >> "%PS_PATH%"
echo     Write-Step "Cleaning up..." >> "%PS_PATH%"
echo     Remove-Item $desktopPath -Force -ErrorAction SilentlyContinue >> "%PS_PATH%"
echo     Write-Host "DEPLOYMENT SUCCESSFUL" -ForegroundColor Green >> "%PS_PATH%"
echo } catch { Write-Host "!!! FAILED !!!" -ForegroundColor Red ; Write-Host $_.Exception.Message -ForegroundColor Yellow } >> "%PS_PATH%"
echo Read-Host "Press Enter to finish" >> "%PS_PATH%"

powershell -NoProfile -ExecutionPolicy Bypass -File "%PS_PATH%"
if exist "%PS_PATH%" del "%PS_PATH%"
pause
