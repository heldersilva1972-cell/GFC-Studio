@echo off
setlocal

:: Get the IP from the user's recent screenshot
set SERVER_IP=192.168.0.72
set DOMAIN=gfc.lovanow.com

echo ========================================================
echo GFC LAN Access Fixer (NAT Loopback Bypass)
echo ========================================================
echo This script will point %DOMAIN% to your local server (%SERVER_IP%)
echo so you can access it while on the club WiFi.
echo.

:: Check for Administrator privileges
net session >nul 2>&1
if %errorLevel% == 0 (
    echo Administrator privileges confirmed.
) else (
    echo [ERROR] You must run this file as ADMINISTRATOR.
    echo Right-click the file and select "Run as Administrator".
    pause
    exit /b
)

echo Adding entry to hosts file...
:: Backup first
copy %windir%\system32\drivers\etc\hosts %windir%\system32\drivers\etc\hosts.bak >nul

:: Check if entry already exists
findstr /i "%DOMAIN%" %windir%\system32\drivers\etc\hosts >nul
if %errorLevel% == 0 (
    echo [INFO] Entry for %DOMAIN% already exists. Cleaning up old entry...
    powershell -Command "(gc %windir%\system32\drivers\etc\hosts) -replace '.*%DOMAIN%.*', '%SERVER_IP% %DOMAIN%' | Out-File %windir%\system32\drivers\etc\hosts -Encoding ascii"
) else (
    echo %SERVER_IP% %DOMAIN% >> %windir%\system32\drivers\etc\hosts
)

echo [SUCCESS] Hosts file updated.
echo Flushing DNS...
ipconfig /flushdns >nul

echo ========================================================
echo Done! You should now be able to visit:
echo https://%DOMAIN%
echo ========================================================
echo (Note: If you leave the club network, you may need to remove
echo this line or add a '#' in front of it in the hosts file.)
echo.
pause
