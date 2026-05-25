@echo off
title GFC POS PUBLISHER (FULLY AUTOMATED)
echo.
echo ===================================================
echo   GFC POS Publisher (Run on Dev Machine)
echo ===================================================

:: 1. Sync versions automatically from PosVersion.props
echo Syncing versions from PosVersion.props...
set "PROPS_PATH=apps\GFC-Pos-Standalone\PosVersion.props"
for /f "tokens=3 delims=><" %%v in ('findstr "PosVersion" "%PROPS_PATH%"') do set "PosVersion=%%v"

if "%PosVersion%"=="" (
    echo [ERROR] Could not find version in %PROPS_PATH%
    pause
    exit /b
)

powershell -ExecutionPolicy Bypass -File "sync-version.ps1" -Project POS -Version "%PosVersion%"
if %errorLevel% neq 0 (
    echo [ERROR] Version sync failed.
    pause
    exit /b
)

:: 2. Determine target build track
set "TRACK="
if /i "%1"=="-mobile" set "TRACK=mobile"
if /i "%1"=="-apk" set "TRACK=mobile"
if /i "%1"=="-web" set "TRACK=web"
if /i "%1"=="-pwa" set "TRACK=web"

if "%TRACK%"=="" (
    echo Please select the build track:
    echo [1] Standard Web/PWA (default)
    echo [2] Native Android (APK)
    set /p choice="Enter choice (1 or 2) [1]: "
    if "%choice%"=="2" (
        set "TRACK=mobile"
    ) else (
        set "TRACK=web"
    )
)

:: Get the new version for the name
set "PROPS_PATH=apps\GFC-Pos-Standalone\PosVersion.props"
for /f "tokens=3 delims=><" %%v in ('findstr "PosVersion" "%PROPS_PATH%"') do set "PosVersion=%%v"

echo Publishing Version: %PosVersion% (Track: %TRACK%)

if "%TRACK%"=="mobile" (
    echo Cleaning old build artifacts...
    if exist "publish_pos_mobile" rd /s /q "publish_pos_mobile"
    
    echo Running dotnet publish for Native Android project...
    dotnet publish "apps\GFC-Pos-Standalone\GFC.Pos.Mobile\GFC.Pos.Mobile.csproj" -f net10.0-android -c Release -o "publish_pos_mobile" /p:TreatWarningsAsErrors=false
    if %errorLevel% neq 0 (
        echo.
        echo [CRITICAL ERROR] dotnet publish FAILED for Native Android.
        pause
        exit /b
    )
    
    echo Locating generated APK installer file...
    powershell -Command "$apk = Get-ChildItem -Path 'publish_pos_mobile\*.apk' | Select-Object -First 1; if (-not $apk) { $apk = Get-ChildItem -Path 'apps\GFC-Pos-Standalone\GFC.Pos.Mobile\bin\Release\net10.0-android\*.apk' | Select-Object -First 1 }; if ($apk) { Copy-Item $apk.FullName -Destination '%USERPROFILE%\Desktop\GFC_POS_Mobile.apk' -Force; Write-Host 'Copied' $apk.Name 'to Desktop as GFC_POS_Mobile.apk' } else { Write-Error 'No APK file found in publish outputs!' }"
    if %errorLevel% neq 0 (
        echo [ERROR] Failed to locate or copy APK installer file.
        pause
        exit /b
    )
    
    if exist "publish_pos_mobile" rd /s /q "publish_pos_mobile"
    echo.
    echo SUCCESS! GFC_POS_Mobile.apk (v%PosVersion%) is on your Desktop.
    echo Copy to server and run Deploy_POS.bat.
    pause
    exit /b
)

:: --- STANDARD WEB/PWA TRACK ---
echo Cleaning old build artifacts...
if exist "publish_pos" rd /s /q "publish_pos"

:: 3. Run dotnet publish
echo Running dotnet publish...
dotnet publish "apps\GFC-Pos-Standalone\GFC.Pos.Terminal\GFC.Pos.Terminal.csproj" -c Release -o "publish_pos" /p:TreatWarningsAsErrors=false
if %errorLevel% neq 0 (
    echo.
    echo [CRITICAL ERROR] dotnet publish FAILED. 
    pause
    exit /b
)

:: 4. Create version.txt inside wwwroot
:: Ensure no trailing space
echo %PosVersion%>"publish_pos\wwwroot\version.txt"

:: 5. Zip it up
echo Zipping to desktop...
powershell -Command "Compress-Archive -Path 'publish_pos\wwwroot\*' -DestinationPath '%USERPROFILE%\Desktop\GFC_POS_Standalone.zip' -Force"

rd /s /q "publish_pos"
echo.
echo SUCCESS! GFC_POS_Standalone.zip (v%PosVersion%) is on your Desktop.
echo Copy to server and run Deploy_POS.bat.
pause
