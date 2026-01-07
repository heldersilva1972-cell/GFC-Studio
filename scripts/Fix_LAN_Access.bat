@echo off
setlocal

:: The NEW detected server IP
set SERVER_IP=192.168.0.248
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

echo Updating entry in hosts file...
:: Backup first
copy %windir%\system32\drivers\etc\hosts %windir%\system32\drivers\etc\hosts.bak >nul

:: Clean up ANY old entry for this domain and add the new one
powershell -Command "$hosts = Get-Content %windir%\system32\drivers\etc\hosts; $newHosts = $hosts | Where-Object { $_ -notmatch '%DOMAIN%' }; $newHosts += '%SERVER_IP% %DOMAIN%'; [System.IO.File]::WriteAllLines('%windir%\system32\drivers\etc\hosts', $newHosts, [System.Text.Encoding]::ASCII)"

echo [SUCCESS] Hosts file updated to point to %SERVER_IP%.
echo Flushing DNS...
ipconfig /flushdns >nul

echo ========================================================
echo Done! You should now be able to visit:
echo https://%DOMAIN%
echo ========================================================
echo.
pause
