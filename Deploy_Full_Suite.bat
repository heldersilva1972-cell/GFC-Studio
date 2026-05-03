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

echo $zipName = "GFC_Full_Suite_Update.zip" >> "%PS_PATH%"
echo $folders = @([Environment]::GetFolderPath("Desktop"), "C:\Users\Lovanow\Desktop", "C:\Users\hnsil\Desktop", ".") >> "%PS_PATH%"
echo function Write-Step($msg, $color = "Cyan") { Write-Host "" ; Write-Host ">>> $msg" -ForegroundColor $color } >> "%PS_PATH%"
echo try { >> "%PS_PATH%"
echo     $desktopPath = $null >> "%PS_PATH%"
echo     foreach ($f in $folders) { if ($f -and (Test-Path $f)) { $tp = Join-Path $f $zipName ; if (Test-Path $tp) { $desktopPath = $tp; break } } } >> "%PS_PATH%"
echo     if (-not $desktopPath) { Write-Host "[INFO] No $zipName found." -ForegroundColor Yellow ; Read-Host "Press Enter" ; return } >> "%PS_PATH%"
echo     $staging = "C:\inetpub\PublishFullSuiteStaging" >> "%PS_PATH%"
echo     Write-Step "Unzipping Full Suite..." >> "%PS_PATH%"
echo     if (Test-Path $staging) { Remove-Item $staging -Recurse -Force -ErrorAction SilentlyContinue } >> "%PS_PATH%"
echo     Expand-Archive -Path $desktopPath -DestinationPath $staging -Force >> "%PS_PATH%"
echo     $apps = @( >> "%PS_PATH%"
echo         @{ Name="GFCWebApp"; Live="C:\inetpub\GFCWebApp"; Staging="webapp"; Backup="C:\inetpub\history_webapp" }, >> "%PS_PATH%"
echo         @{ Name="GFCMobile"; Live="C:\inetpub\GFCMobile"; Staging="mobile"; Backup="C:\inetpub\history_mobile" }, >> "%PS_PATH%"
echo         @{ Name="GFCPos";    Live="C:\inetpub\GFCPos";    Staging="pos";    Backup="C:\inetpub\history_pos" } >> "%PS_PATH%"
echo     ) >> "%PS_PATH%"
echo     Import-Module WebAdministration -ErrorAction SilentlyContinue >> "%PS_PATH%"
echo     foreach ($app in $apps) { >> "%PS_PATH%"
echo         $appStaging = Join-Path $staging $app.Staging >> "%PS_PATH%"
echo         if (-not (Test-Path $appStaging)) { Write-Host "Skipping $($app.Name) - No staging folder" ; continue } >> "%PS_PATH%"
echo         Write-Step "Deploying $($app.Name)..." "Yellow" >> "%PS_PATH%"
echo         try { Stop-Website $app.Name ; Stop-WebAppPool $app.Name } catch {} >> "%PS_PATH%"
echo         if (-not (Test-Path $app.Backup)) { New-Item -ItemType Directory -Path $app.Backup ^| Out-Null } >> "%PS_PATH%"
echo         $ts = Get-Date -Format "yyyyMMdd_HHmmss" >> "%PS_PATH%"
echo         $bp = Join-Path $app.Backup "Backup_$ts" >> "%PS_PATH%"
echo         if (Test-Path $app.Live) { Copy-Item -Path "$($app.Live)\*" -Destination $bp -Recurse -Force -ErrorAction SilentlyContinue } >> "%PS_PATH%"
echo         $items = Get-ChildItem -Path $appStaging -Recurse >> "%PS_PATH%"
echo         foreach ($item in $items) { >> "%PS_PATH%"
echo             if ($item.Name -eq "appsettings.Production.json") { continue } >> "%PS_PATH%"
echo             $rel = $item.FullName.Substring($appStaging.Length).TrimStart("\") >> "%PS_PATH%"
echo             $dest = Join-Path $app.Live $rel >> "%PS_PATH%"
echo             if ($item.PSIsContainer) { if (-not (Test-Path $dest)) { New-Item -ItemType Directory -Path $dest ^| Out-Null } } >> "%PS_PATH%"
echo             else { if (-not (Test-Path (Split-Path $dest))) { New-Item -ItemType Directory -Path (Split-Path $dest) ^| Out-Null } ; Copy-Item -Path $item.FullName -Destination $dest -Force } >> "%PS_PATH%"
echo         } >> "%PS_PATH%"
echo         try { Start-WebAppPool $app.Name ; Start-Website $app.Name } catch {} >> "%PS_PATH%"
echo     } >> "%PS_PATH%"
echo     Write-Step "Cleaning up..." >> "%PS_PATH%"
echo     Remove-Item $staging -Recurse -Force >> "%PS_PATH%"
echo     Write-Host "FULL SUITE DEPLOYMENT SUCCESSFUL" -ForegroundColor Green >> "%PS_PATH%"
echo } catch { Write-Host "!!! FAILED !!!" -ForegroundColor Red ; Write-Host $_.Exception.Message -ForegroundColor Yellow } >> "%PS_PATH%"
echo Read-Host "Press Enter to finish" >> "%PS_PATH%"

powershell -NoProfile -ExecutionPolicy Bypass -File "%PS_PATH%"
if exist "%PS_PATH%" del "%PS_PATH%"
pause
