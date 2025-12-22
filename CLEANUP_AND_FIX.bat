@echo off
echo Stopping all conflicting website servers...
taskkill /F /IM dotnet.exe >nul 2>&1
taskkill /F /IM python.exe >nul 2>&1
taskkill /F /IM powershell.exe >nul 2>&1
echo.
echo ✅ Cleaned up!
echo.
echo NOW:
echo 1. Restart your Web App in Visual Studio.
echo 2. Click "Public Website" in the menu.
echo 3. IT WILL WORK.
echo.
pause
