@echo off
setlocal
echo ========================================
echo GFC FULL SUITE PUBLISH TOOL
echo ========================================
cd /d "%~dp0"

echo [1/3] Publishing WebApp...
dotnet publish "apps\webapp\GFC.BlazorServer\GFC.BlazorServer.csproj" -c Release -o "publish_output\webapp"

echo [2/3] Publishing Mobile Standalone...
dotnet publish "apps\GFC-Mobile-Standalone\GFC.Mobile\GFC.Mobile.csproj" -c Release -o "publish_output\mobile"

echo [3/3] Publishing POS Standalone (Web Terminal)...
dotnet publish "apps\GFC-Pos-Standalone\GFC.Pos.Terminal\GFC.Pos.Terminal.csproj" -c Release -o "publish_output\pos"

if errorlevel 1 (
    echo [ERROR] One or more builds failed.
    pause
    exit /b
)

echo Zipping Full Suite to Desktop...
powershell -Command "Compress-Archive -Path 'publish_output\*' -DestinationPath '%USERPROFILE%\Desktop\GFC_Full_Suite_Update.zip' -Force"

if exist "publish_output" rd /s /q "publish_output"

echo ========================================
echo SUCCESS! Full Suite Zip is on your desktop.
echo ========================================
pause
