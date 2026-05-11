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

:: 2. Get the new version for the ZIP name
set "PROPS_PATH=apps\GFC-Pos-Standalone\PosVersion.props"
for /f "tokens=3 delims=><" %%v in ('findstr "PosVersion" "%PROPS_PATH%"') do set "PosVersion=%%v"

echo Publishing Version: %PosVersion%
echo Cleaning old build artifacts...
if exist "publish_pos" rd /s /q "publish_pos"

:: 3. Run dotnet publish
echo Running dotnet publish...
dotnet publish "apps\GFC-Pos-Standalone\GFC.Pos.Terminal\GFC.Pos.Terminal.csproj" -c Release -o "publish_pos"
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
