@echo off
setlocal

set DOMAIN=gfc.lovanow.com

echo ========================================================
echo GFC Public Access Restorer
echo ========================================================
echo This script will reset %DOMAIN% to use normal internet DNS.
echo Run this when you are NOT at the club.
echo.

:: Check for Administrator privileges
net session >nul 2>&1
if %errorLevel% == 0 (
    echo Administrator privileges confirmed.
) else (
    echo [ERROR] You must run this file as ADMINISTRATOR.
    echo.
    pause
    exit /b
)

echo Cleaning up hosts file...
powershell -Command "$hosts = Get-Content %windir%\system32\drivers\etc\hosts; $newHosts = $hosts | Where-Object { $_ -notmatch '%DOMAIN%' }; [System.IO.File]::WriteAllLines('%windir%\system32\drivers\etc\hosts', $newHosts, [System.Text.Encoding]::ASCII)"

echo [SUCCESS] Hosts file cleaned.
echo Flushing DNS...
ipconfig /flushdns >nul

echo ========================================================
echo Done! Your laptop will now use Cloudflare for:
echo https://%DOMAIN%
echo ========================================================
echo.
pause
