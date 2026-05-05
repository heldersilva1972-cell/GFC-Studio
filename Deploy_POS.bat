@echo off
title GFC POS Deployer
echo.
echo ===================================================
echo   GFC POS Deployment Initializing...
echo ===================================================

:: --- CONFIGURATION ---
set "SITE_NAME=GFC_POS"
set "LIVE_PATH=C:\inetpub\wwwroot\GFCPOS"
:: ---------------------

net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [CRITICAL ERROR] This must be run as ADMINISTRATOR.
    pause
    exit /b
)

set "PS_PATH=%TEMP%\gfc_pos_deploy.ps1"
if exist "%PS_PATH%" del "%PS_PATH%"

>>"%PS_PATH%" echo $zipName = "GFC_POS_Standalone.zip"
>>"%PS_PATH%" echo $folders = @([Environment]::GetFolderPath("Desktop"), "C:\Users\Lovanow\Desktop", "C:\Users\hnsil\Desktop", ".")
>>"%PS_PATH%" echo $siteName = "%SITE_NAME%"
>>"%PS_PATH%" echo $live = "%LIVE_PATH%"
>>"%PS_PATH%" echo function Write-Step($msg, $color = "Cyan") { Write-Host "" ; Write-Host ">>> $msg" -ForegroundColor $color }
>>"%PS_PATH%" echo try {
>>"%PS_PATH%" echo     $desktopPath = $null
>>"%PS_PATH%" echo     foreach ($f in $folders) { if ($f -and (Test-Path $f)) { $tp = Join-Path $f $zipName ; if (Test-Path $tp) { $desktopPath = $tp; break } } }
>>"%PS_PATH%" echo     if (-not $desktopPath) { Write-Host "[INFO] No $zipName found." -ForegroundColor Yellow ; Read-Host "Press Enter" ; return }
>>"%PS_PATH%" echo     $staging = "C:\inetpub\PublishGFCPos"
>>"%PS_PATH%" echo     $backupRoot = "C:\inetpub\history_pos"
>>"%PS_PATH%" echo     Write-Step "Preparing staging..."
>>"%PS_PATH%" echo     if (Test-Path $staging) { Remove-Item $staging -Recurse -Force -ErrorAction SilentlyContinue }
>>"%PS_PATH%" echo     New-Item -ItemType Directory -Path $staging ^| Out-Null
>>"%PS_PATH%" echo     Write-Step "Unzipping..."
>>"%PS_PATH%" echo     Expand-Archive -Path $desktopPath -DestinationPath $staging -Force
>>"%PS_PATH%" echo     Write-Step "Ensuring IIS Provider..."
>>"%PS_PATH%" echo     Import-Module WebAdministration -ErrorAction SilentlyContinue
>>"%PS_PATH%" echo     Write-Step "Stopping IIS ($siteName)..."
>>"%PS_PATH%" echo     if (Get-Website -Name $siteName -ErrorAction SilentlyContinue) { Stop-Website $siteName -ErrorAction SilentlyContinue }
>>"%PS_PATH%" echo     if (Test-Path "IIS:\AppPools\$siteName") { Stop-WebAppPool $siteName -ErrorAction SilentlyContinue }
>>"%PS_PATH%" echo     Start-Sleep -Seconds 2
>>"%PS_PATH%" echo     Write-Step "Backing up site..."
>>"%PS_PATH%" echo     if (-not (Test-Path $backupRoot)) { New-Item -ItemType Directory -Path $backupRoot ^| Out-Null }
>>"%PS_PATH%" echo     $ts = Get-Date -Format "yyyyMMdd_HHmmss"
>>"%PS_PATH%" echo     $bp = Join-Path $backupRoot "Backup_$ts"
>>"%PS_PATH%" echo     if (Test-Path $live) { Copy-Item -Path "$live\*" -Destination $bp -Recurse -Force -ErrorAction SilentlyContinue }
>>"%PS_PATH%" echo     Write-Step "Deploying files to $live..."
>>"%PS_PATH%" echo     # SEARCH for index.html to find the correct source root
>>"%PS_PATH%" echo     $sourcePath = $staging
>>"%PS_PATH%" echo     $indexFile = Get-ChildItem -Path $staging -Filter "index.html" -Recurse ^| Select-Object -First 1
>>"%PS_PATH%" echo     if ($indexFile) { $sourcePath = $indexFile.DirectoryName }
>>"%PS_PATH%" echo     Write-Host "[INFO] Source root identified as: $sourcePath" -ForegroundColor Gray
>>"%PS_PATH%" echo     robocopy $sourcePath $live /S /E /PURGE /XF "appsettings.Production.json" "web.config" ^| Out-Null
>>"%PS_PATH%" echo     Write-Step "Generating Web.Config..."
>>"%PS_PATH%" echo     $liveWebConfig = Join-Path $live "web.config"
>>"%PS_PATH%" echo     $cleanWebConfig = @"
>>"%PS_PATH%" echo ^<?xml version="1.0" encoding="UTF-8"?^>
>>"%PS_PATH%" echo ^<configuration^>
>>"%PS_PATH%" echo   ^<system.webServer^>
>>"%PS_PATH%" echo     ^<staticContent^>
>>"%PS_PATH%" echo       ^<remove fileExtension=".blat" /^>
>>"%PS_PATH%" echo       ^<remove fileExtension=".dat" /^>
>>"%PS_PATH%" echo       ^<remove fileExtension=".dll" /^>
>>"%PS_PATH%" echo       ^<remove fileExtension=".webcil" /^>
>>"%PS_PATH%" echo       ^<remove fileExtension=".json" /^>
>>"%PS_PATH%" echo       ^<remove fileExtension=".wasm" /^>
>>"%PS_PATH%" echo       ^<remove fileExtension=".woff" /^>
>>"%PS_PATH%" echo       ^<remove fileExtension=".woff2" /^>
>>"%PS_PATH%" echo       ^<mimeMap fileExtension=".blat" mimeType="application/octet-stream" /^>
>>"%PS_PATH%" echo       ^<mimeMap fileExtension=".dll" mimeType="application/octet-stream" /^>
>>"%PS_PATH%" echo       ^<mimeMap fileExtension=".webcil" mimeType="application/octet-stream" /^>
>>"%PS_PATH%" echo       ^<mimeMap fileExtension=".dat" mimeType="application/octet-stream" /^>
>>"%PS_PATH%" echo       ^<mimeMap fileExtension=".json" mimeType="application/json" /^>
>>"%PS_PATH%" echo       ^<mimeMap fileExtension=".wasm" mimeType="application/wasm" /^>
>>"%PS_PATH%" echo       ^<mimeMap fileExtension=".woff" mimeType="application/font-woff" /^>
>>"%PS_PATH%" echo       ^<mimeMap fileExtension=".woff2" mimeType="application/font-woff" /^>
>>"%PS_PATH%" echo       ^<remove fileExtension=".webmanifest" /^>
>>"%PS_PATH%" echo       ^<mimeMap fileExtension=".webmanifest" mimeType="application/manifest+json" /^>
>>"%PS_PATH%" echo     ^</staticContent^>
>>"%PS_PATH%" echo     ^<rewrite^>
>>"%PS_PATH%" echo       ^<rules^>
>>"%PS_PATH%" echo         ^<rule name="SPA fallback routing" stopProcessing="true"^>
>>"%PS_PATH%" echo           ^<match url=".*" /^>
>>"%PS_PATH%" echo           ^<conditions logicalGrouping="MatchAll"^>
>>"%PS_PATH%" echo             ^<add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" /^>
>>"%PS_PATH%" echo           ^</conditions^>
>>"%PS_PATH%" echo           ^<action type="Rewrite" url="/" /^>
>>"%PS_PATH%" echo         ^</rule^>
>>"%PS_PATH%" echo       ^</rules^>
>>"%PS_PATH%" echo     ^</rewrite^>
>>"%PS_PATH%" echo   ^</system.webServer^>
>>"%PS_PATH%" echo ^</configuration^>
>>"%PS_PATH%" echo "@
>>"%PS_PATH%" echo     Set-Content -Path $liveWebConfig -Value $cleanWebConfig -Encoding UTF8
>>"%PS_PATH%" echo     Write-Step "Starting IIS Resources ($siteName)..." "Green"
>>"%PS_PATH%" echo     if (Test-Path "IIS:\AppPools\$siteName") { Start-WebAppPool $siteName }
>>"%PS_PATH%" echo     if (Get-Website -Name $siteName -ErrorAction SilentlyContinue) { Start-Website $siteName }
>>"%PS_PATH%" echo     Write-Step "Cleaning up..."
>>"%PS_PATH%" echo     Remove-Item $desktopPath -Force -ErrorAction SilentlyContinue
>>"%PS_PATH%" echo     Write-Host "DEPLOYMENT SUCCESSFUL" -ForegroundColor Green
>>"%PS_PATH%" echo } catch { Write-Host "!!! FAILED !!!" -ForegroundColor Red ; Write-Host $_.Exception.Message -ForegroundColor Yellow }
>>"%PS_PATH%" echo Read-Host "Press Enter to finish"

powershell -NoProfile -ExecutionPolicy Bypass -File "%PS_PATH%"
if exist "%PS_PATH%" del "%PS_PATH%"
pause
