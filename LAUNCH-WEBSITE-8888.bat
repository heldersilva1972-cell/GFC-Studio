@echo off
title GFC Website Server (Port 8888)
echo ==============================================
echo       STARTING GFC WEBSITE SERVER
echo        http://localhost:8888
echo ==============================================
echo.

REM Kill any existing process on 8888 or 3000 just in case
taskkill /f /im dotnet.exe >nul 2>&1

echo Building server...
cd /d "%~dp0apps\simple-server"
dotnet build
echo.
echo Starting server...
echo.
echo Website is at: http://localhost:8888
echo.
start http://localhost:8888
dotnet run
pause
