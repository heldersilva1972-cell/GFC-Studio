@echo off
taskkill /F /IM dotnet.exe >nul 2>&1
echo Starting Website...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0ForceStartWebsite.ps1"
pause
