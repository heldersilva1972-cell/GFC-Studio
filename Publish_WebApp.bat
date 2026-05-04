@echo off
setlocal
cd /d "%~dp0"

echo Stopping GFC WebApp if running...
taskkill /IM GFC.BlazorServer.exe /F >nul 2>&1
timeout /t 2 /nobreak >nul

echo Publishing Main WebApp...
dotnet publish "apps\webapp\GFC.BlazorServer\GFC.BlazorServer.csproj" -c Release -o "publish_webapp"
if %ERRORLEVEL% neq 0 (
    echo.
    echo BUILD FAILED. Check the errors above.
    pause
    exit /b 1
)

powershell -Command "Compress-Archive -Path 'publish_webapp\*' -DestinationPath '%USERPROFILE%\Desktop\PublishGFCWebApp.zip' -Force"
rd /s /q "publish_webapp"
echo.
echo DONE! Zip is on your desktop.
pause
