@echo off
title GFC Full Suite Deployer
echo.
echo ===================================================
echo   GFC FULL SUITE Deployment Initializing...
echo ===================================================

net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [CRITICAL ERROR] This must be run as ADMINISTRATOR.
    pause
    exit /b
)

set "PS_PATH=%TEMP%\gfc_full_deploy.ps1"
if exist "%PS_PATH%" del "%PS_PATH%"

>>"%PS_PATH%" echo $zipName = "GFC_Full_Suite_Update.zip"
>>"%PS_PATH%" echo $folders = @([Environment]::GetFolderPath("Desktop"), "C:\Users\Lovanow\Desktop", "C:\Users\hnsil\Desktop", ".")
>>"%PS_PATH%" echo function Write-Step($msg, $color = "Cyan") { Write-Host "" ; Write-Host ">>> $msg" -ForegroundColor $color }
>>"%PS_PATH%" echo try {
>>"%PS_PATH%" echo     $desktopPath = $null
>>"%PS_PATH%" echo     foreach ($f in $folders) { if ($f -and (Test-Path $f)) { $tp = Join-Path $f $zipName ; if (Test-Path $tp) { $desktopPath = $tp; break } } }
>>"%PS_PATH%" echo     if (-not $desktopPath) { Write-Host "[INFO] No $zipName found." -ForegroundColor Yellow ; Read-Host "Press Enter" ; return }
>>"%PS_PATH%" echo     $staging = "C:\inetpub\PublishFullSuiteStaging"
>>"%PS_PATH%" echo     Write-Step "Unzipping Full Suite..."
>>"%PS_PATH%" echo     if (Test-Path $staging) { Remove-Item $staging -Recurse -Force -ErrorAction SilentlyContinue }
>>"%PS_PATH%" echo     Expand-Archive -Path $desktopPath -DestinationPath $staging -Force
>>"%PS_PATH%" echo     $apps = @(
>>"%PS_PATH%" echo         @{ Name="GFCWebApp"; Live="C:\inetpub\GFCWebApp"; Staging="webapp"; Backup="C:\inetpub\history_webapp" },
>>"%PS_PATH%" echo         @{ Name="GFCMobile"; Live="C:\inetpub\wwwroot\GFCMobile"; Staging="mobile"; Backup="C:\inetpub\history_mobile" },
>>"%PS_PATH%" echo         @{ Name="GFCPOS";    Live="C:\inetpub\wwwroot\GFCPOS";    Staging="pos";    Backup="C:\inetpub\history_pos" }
>>"%PS_PATH%" echo     )
>>"%PS_PATH%" echo     Import-Module WebAdministration -ErrorAction SilentlyContinue
>>"%PS_PATH%" echo     foreach ($app in $apps) {
>>"%PS_PATH%" echo         $appStaging = Join-Path $staging $app.Staging
>>"%PS_PATH%" echo         if (-not (Test-Path $appStaging)) { Write-Host "Skipping $($app.Name) - No staging folder" ; continue }
>>"%PS_PATH%" echo         Write-Step "Deploying $($app.Name)..." "Yellow"
>>"%PS_PATH%" echo         try { Stop-Website $app.Name ; Stop-WebAppPool $app.Name ; Start-Sleep -s 5 } catch {}
>>"%PS_PATH%" echo         if (-not (Test-Path $app.Backup)) { New-Item -ItemType Directory -Path $app.Backup ^| Out-Null }
>>"%PS_PATH%" echo         $ts = Get-Date -Format "yyyyMMdd_HHmmss"
>>"%PS_PATH%" echo         $bp = Join-Path $app.Backup "Backup_$ts"
>>"%PS_PATH%" echo         if (Test-Path $app.Live) { Copy-Item -Path "$($app.Live)\*" -Destination $bp -Recurse -Force -ErrorAction SilentlyContinue }
>>"%PS_PATH%" echo         if ($app.Name -eq "GFCMobile" -or $app.Name -eq "GFCPOS") {
>>"%PS_PATH%" echo             Write-Step "Flattening $($app.Name) deployment..."
>>"%PS_PATH%" echo             $sourceWwwroot = Join-Path $appStaging "wwwroot"
>>"%PS_PATH%" echo             robocopy $sourceWwwroot $app.Live /S /E /PURGE /XD "history" /XF "appsettings.Production.json" "web.config" ^| Out-Null
>>"%PS_PATH%" echo             $liveWebConfig = Join-Path $app.Live "web.config"
>>"%PS_PATH%" echo             $cleanWebConfig = @"
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
>>"%PS_PATH%" echo     ^<httpCompression^>
>>"%PS_PATH%" echo       ^<dynamicTypes^>
>>"%PS_PATH%" echo         ^<add mimeType="application/octet-stream" enabled="true" /^>
>>"%PS_PATH%" echo         ^<add mimeType="application/wasm" enabled="true" /^>
>>"%PS_PATH%" echo       ^</dynamicTypes^>
>>"%PS_PATH%" echo     ^</httpCompression^>
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
>>"%PS_PATH%" echo             Set-Content -Path $liveWebConfig -Value $cleanWebConfig -Encoding UTF8
>>"%PS_PATH%" echo         } else {
>>"%PS_PATH%" echo             $items = Get-ChildItem -Path $appStaging -Recurse
>>"%PS_PATH%" echo             foreach ($item in $items) {
>>"%PS_PATH%" echo                 if ($item.Name -eq "appsettings.Production.json") { continue }
>>"%PS_PATH%" echo                 $rel = $item.FullName.Substring($appStaging.Length).TrimStart("\")
>>"%PS_PATH%" echo                 $dest = Join-Path $app.Live $rel
>>"%PS_PATH%" echo                 if ($item.PSIsContainer) { if (-not (Test-Path $dest)) { New-Item -ItemType Directory -Path $dest ^| Out-Null } }
>>"%PS_PATH%" echo                 else { if (-not (Test-Path (Split-Path $dest))) { New-Item -ItemType Directory -Path (Split-Path $dest) ^| Out-Null } ; Copy-Item -Path $item.FullName -Destination $dest -Force }
>>"%PS_PATH%" echo             }
>>"%PS_PATH%" echo         }
>>"%PS_PATH%" echo         try { Start-WebAppPool $app.Name ; Start-Website $app.Name } catch {}
>>"%PS_PATH%" echo     }
>>"%PS_PATH%" echo     Write-Step "Cleaning up..."
>>"%PS_PATH%" echo     Remove-Item $staging -Recurse -Force
>>"%PS_PATH%" echo     Write-Host "FULL SUITE DEPLOYMENT SUCCESSFUL" -ForegroundColor Green
>>"%PS_PATH%" echo     if (Test-Path $desktopPath) { Remove-Item $desktopPath -Force }
>>"%PS_PATH%" echo } catch { 
>>"%PS_PATH%" echo     Write-Host "!!! FAILED !!!" -ForegroundColor Red ; Write-Host $_.Exception.Message -ForegroundColor Yellow 
>>"%PS_PATH%" echo     Write-Host "Restarting apps..." -ForegroundColor Cyan
>>"%PS_PATH%" echo     foreach ($app in $apps) { try { Start-WebAppPool $app.Name ; Start-Website $app.Name } catch {} }
>>"%PS_PATH%" echo }
>>"%PS_PATH%" echo Read-Host "Press Enter to finish"

powershell -NoProfile -ExecutionPolicy Bypass -File "%PS_PATH%"
if exist "%PS_PATH%" del "%PS_PATH%"
pause
