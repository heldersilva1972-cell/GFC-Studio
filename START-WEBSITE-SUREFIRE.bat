@echo off
title GFC Website Server (Do Not Close)
echo.
echo ==============================================
echo   STARTING GFC WEBSITE SERVER
echo ==============================================
echo.
echo This uses .NET which is already installed on your system.
echo.

cd /d "%~dp0apps\simple-server"
dotnet run

if errorlevel 1 (
    echo.
    echo ERROR: Could not start the server.
    echo Please make sure you are not running another server on port 3000.
    pause
)
