@echo off
setlocal
cd /d "%~dp0"

echo Stopping GFC POS if running...
taskkill /IM GFC.Pos.Mobile.exe /F >nul 2>&1
timeout /t 2 /nobreak >nul

echo Publishing POS Standalone...
dotnet publish "apps\GFC-Pos-Standalone\GFC.Pos.Mobile\GFC.Pos.Mobile.csproj" -c Release -o "publish_pos"
if %ERRORLEVEL% neq 0 (
    echo.
    echo BUILD FAILED. Check the errors above.
    pause
    exit /b 1
)

powershell -Command "Compress-Archive -Path 'publish_pos\*' -DestinationPath '%USERPROFILE%\Desktop\GFC_POS_Standalone.zip' -Force"
rd /s /q "publish_pos"
echo.
echo DONE! Zip is on your desktop.
pause
