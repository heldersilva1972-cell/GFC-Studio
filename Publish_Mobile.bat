@echo off
setlocal
cd /d "%~dp0"

echo Stopping GFC Mobile if running...
taskkill /IM GFC.Mobile.exe /F >nul 2>&1
timeout /t 2 /nobreak >nul

echo Publishing Mobile Standalone...
dotnet publish "apps\GFC-Mobile-Standalone\GFC.Mobile\GFC.Mobile.csproj" -c Release -o "publish_mobile"
if %ERRORLEVEL% neq 0 (
    echo.
    echo BUILD FAILED. Check the errors above.
    pause
    exit /b 1
)

powershell -Command "Compress-Archive -Path 'publish_mobile\*' -DestinationPath '%USERPROFILE%\Desktop\GFC_Mobile_Standalone.zip' -Force"
rd /s /q "publish_mobile"
echo.
echo DONE! Zip is on your desktop.
pause
