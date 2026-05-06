@echo off
setlocal
echo ========================================
echo GFC FULL SUITE PUBLISH TOOL
echo ========================================
cd /d "%~dp0"

echo [1/3] Publishing WebApp...
dotnet publish "apps\webapp\GFC.BlazorServer\GFC.BlazorServer.csproj" -c Release -o "publish_output\webapp"
if %errorlevel% neq 0 goto :fail

echo [2/3] Publishing Mobile Standalone...
dotnet publish "apps\GFC-Mobile-Standalone\GFC.Mobile\GFC.Mobile.csproj" -c Release -o "publish_output\mobile"
if %errorlevel% neq 0 goto :fail

echo [3/3] Publishing POS Standalone (Web Terminal)...
dotnet publish "apps\GFC-Pos-Standalone\GFC.Pos.Terminal\GFC.Pos.Terminal.csproj" -c Release -o "publish_output\pos"
if %errorlevel% neq 0 goto :fail

goto :success

:fail
echo.
echo [CRITICAL ERROR] One or more builds failed. Deployment aborted.
pause
exit /b 1

:success

echo Zipping Full Suite to Desktop...
powershell -Command "Compress-Archive -Path 'publish_output\*' -DestinationPath '%USERPROFILE%\Desktop\GFC_Full_Suite_Update.zip' -Force"

if exist "publish_output" rd /s /q "publish_output"

echo ========================================
echo SUCCESS! Full Suite Zip is on your desktop.
echo ========================================
pause
