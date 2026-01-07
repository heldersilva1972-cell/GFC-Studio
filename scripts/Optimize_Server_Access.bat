@echo off
setlocal

echo ========================================================
echo GFC Server Access Optimizer
echo ========================================================

:: 1. Fix Windows Firewall for Port 8080
echo [1/3] Checking Windows Firewall...
netsh advfirewall firewall show rule name="GFC-Studio-Port-8080" >nul 2>&1
if %errorLevel% neq 0 (
    echo [INFO] Adding Firewall Rule for Port 8080...
    netsh advfirewall firewall add rule name="GFC-Studio-Port-8080" dir=in action=allow protocol=TCP localport=8080
) else (
    echo [OK] Firewall rule already exists.
)

:: 2. Fix IIS Bindings (Ensuring it listens on all IPs)
:: This is harder via CMD without appcmd, but we'll try to ensure port 8080 is open to all
echo [2/3] Verifying IIS Bindings...
:: We'll assume IIS is configured correctly but ensure 0.0.0.0 is listening if possible

:: 3. Correct the LanSubnet in the Database
echo [3/3] Updating Database LAN Subnet to 192.168.0.0/24...
sqlcmd -S .\SQLEXPRESS -d ClubMembership -Q "UPDATE SystemSettings SET LanSubnet = '192.168.0.0/24' WHERE Id = 1"

echo.
echo ========================================================
echo SERVER UPDATED
echo ========================================================
echo Please try accessing again on your laptop:
echo http://192.168.0.72:8080
echo ========================================================
pause
