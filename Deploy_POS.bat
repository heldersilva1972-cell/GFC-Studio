@echo off
title GFC POS DEPLOYER (ULTIMATE CONFIG)
echo.
echo ===================================================
echo   GFC POS Deployment (COMPLETE SYSTEM RESTORE)
echo ===================================================

:: --- CONFIGURATION ---
set "LIVE_PATH=C:\inetpub\wwwroot\GFCPOS"
set "STAGING_PATH=C:\inetpub\PublishGFCPos"
:: ---------------------

net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [CRITICAL ERROR] This must be run as ADMINISTRATOR.
    pause
    exit /b
)

set "PS_PATH=%TEMP%\gfc_pos_deploy.ps1"
if exist "%PS_PATH%" del "%PS_PATH%"

echo $zipName = "GFC_POS_Standalone.zip">>"%PS_PATH%"
echo $live = "%LIVE_PATH%">>"%PS_PATH%"
echo $staging = "%STAGING_PATH%">>"%PS_PATH%"
echo $searchPaths = @([Environment]::GetFolderPath("Desktop"), "C:\Users\Lovanow\Desktop", "C:\Users\hnsil\Desktop", ".")>>"%PS_PATH%"
echo try {>>"%PS_PATH%"
echo     # 1. CLEANUP PREVIOUS CONFLICTS>>"%PS_PATH%"
echo     if (Test-Path "C:\inetpub\wwwroot\web.config") { Remove-Item "C:\inetpub\wwwroot\web.config" -Force }>>"%PS_PATH%"
echo.
echo     # 2. LOCATE ZIP>>"%PS_PATH%"
echo     $foundZip = $null>>"%PS_PATH%"
echo     foreach ($path in $searchPaths) { if ($path -and (Test-Path $path)) { $fullPath = Join-Path $path $zipName ; if (Test-Path $fullPath) { $foundZip = $fullPath; break } } }>>"%PS_PATH%"
echo     if (-not $foundZip) { Write-Host "!!! ERROR: No ZIP found." -ForegroundColor Red ; return }>>"%PS_PATH%"
echo.
echo     # 3. UNZIP AND AUTO-DETECT ROOT>>"%PS_PATH%"
echo     if (Test-Path $staging) { Remove-Item $staging -Recurse -Force -ErrorAction SilentlyContinue }>>"%PS_PATH%"
echo     Write-Host ">>> Unzipping $foundZip...">>"%PS_PATH%"
echo     Expand-Archive -Path $foundZip -DestinationPath $staging -Force>>"%PS_PATH%"
echo.
echo     $sourcePath = $staging>>"%PS_PATH%"
echo     $indexFile = Get-ChildItem -Path $staging -Filter "index.html" -Recurse ^| Select-Object -First 1 >>"%PS_PATH%"
echo     if ($indexFile) { $sourcePath = $indexFile.DirectoryName }>>"%PS_PATH%"
echo.
echo     # 4. DEPLOY TO LIVE>>"%PS_PATH%"
echo     if (-not (Test-Path $live)) { New-Item -ItemType Directory -Path $live ^| Out-Null }>>"%PS_PATH%"
echo     icacls $live /grant "IIS_IUSRS:(OI)(CI)R" /T /C /Q>>"%PS_PATH%"
echo     Get-ChildItem $live -Include *.br, *.gz -Recurse -ErrorAction SilentlyContinue ^| Remove-Item -Force>>"%PS_PATH%"
echo     Write-Host ">>> Deploying files to $live...">>"%PS_PATH%"
echo     robocopy $sourcePath $live /S /E /PURGE /XF "appsettings.Production.json" "web.config">>"%PS_PATH%"
echo.
echo     # 5. GENERATE ULTIMATE PRODUCTION CONFIG>>"%PS_PATH%"
echo     $webConfigPath = Join-Path $live "web.config">>"%PS_PATH%"
echo     $config = @'>>"%PS_PATH%"
echo ^<?xml version="1.0" encoding="UTF-8"?^>>>"%PS_PATH%"
echo ^<configuration^>>>"%PS_PATH%"
echo   ^<system.webServer^>>>"%PS_PATH%"
echo     ^<defaultDocument^>>>"%PS_PATH%"
echo       ^<files^>>>"%PS_PATH%"
echo         ^<clear /^>>>"%PS_PATH%"
echo         ^<add value="index.html" /^>>>"%PS_PATH%"
echo       ^</files^>>>"%PS_PATH%"
echo     ^</defaultDocument^>>>"%PS_PATH%"
echo     ^<staticContent^>>>"%PS_PATH%"
echo       ^<clientCache cacheControlMode="DisableCache" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".html" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".html" mimeType="text/html" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".css" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".css" mimeType="text/css" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".js" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".js" mimeType="application/javascript" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".json" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".json" mimeType="application/json" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".txt" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".txt" mimeType="text/plain" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".wasm" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".wasm" mimeType="application/wasm" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".dll" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".dll" mimeType="application/octet-stream" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".blat" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".blat" mimeType="application/octet-stream" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".dat" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".dat" mimeType="application/octet-stream" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".webcil" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".webcil" mimeType="application/octet-stream" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".woff" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".woff" mimeType="application/font-woff" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".woff2" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".woff2" mimeType="application/font-woff" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".webmanifest" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".webmanifest" mimeType="application/manifest+json" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".png" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".png" mimeType="image/png" /^>>>"%PS_PATH%"
echo       ^<remove fileExtension=".ico" /^>>>"%PS_PATH%"
echo       ^<mimeMap fileExtension=".ico" mimeType="image/x-icon" /^>>>"%PS_PATH%"
echo     ^</staticContent^>>>"%PS_PATH%"
echo     ^<rewrite^>>>"%PS_PATH%"
echo       ^<rules^>>>"%PS_PATH%"
echo         ^<rule name="SPA fallback" stopProcessing="true"^>>>"%PS_PATH%"
echo           ^<match url=".*" /^>>>"%PS_PATH%"
echo           ^<conditions logicalGrouping="MatchAll"^>>>"%PS_PATH%"
echo             ^<add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" /^>>>"%PS_PATH%"
echo           ^</conditions^>>>"%PS_PATH%"
echo           ^<action type="Rewrite" url="index.html" /^>>>"%PS_PATH%"
echo         ^</rule^>>>"%PS_PATH%"
echo       ^</rules^>>>"%PS_PATH%"
echo     ^</rewrite^>>>"%PS_PATH%"
echo   ^</system.webServer^>>>"%PS_PATH%"
echo ^</configuration^>>>"%PS_PATH%"
echo '@>>"%PS_PATH%"
echo     $config ^| Set-Content $webConfigPath -Force>>"%PS_PATH%"
echo     if (Test-Path $foundZip) { Remove-Item $foundZip -Force; Write-Host ">>> Cleanup: $zipName deleted." -ForegroundColor Gray }>>"%PS_PATH%"
echo     Write-Host "DEPLOYMENT SUCCESSFUL - ALL SYSTEMS RESTORED" -ForegroundColor Green>>"%PS_PATH%"
echo } catch { Write-Host "!!! FAILED !!!" -ForegroundColor Red ; Write-Host $_.Exception.Message -ForegroundColor Yellow }>>"%PS_PATH%"
echo Read-Host "Press Enter to finish">>"%PS_PATH%"

powershell -NoProfile -ExecutionPolicy Bypass -File "%PS_PATH%"
if exist "%PS_PATH%" del "%PS_PATH%"
pause
