@echo off
echo ===================================================
echo GFC SQL Connection Fixer
echo ===================================================
echo.

echo [1/3] Checking SQL Server Services...
net start | findstr /i "SQL" > nul
if %errorlevel% neq 0 (
    echo [!] No SQL services are running. Attempting to start SQLEXPRESS...
    net start MSSQL$SQLEXPRESS
    net start MSSQLSERVER
) else (
    echo [OK] SQL Services are running.
)

echo.
echo [2/3] Detecting SQL Instances...
echo Available Instances:
powershell -Command "Get-Service | Where-Object {$_.Name -like 'MSSQL*'} | Select-Object Name, Status, DisplayName"
echo.

echo [3/3] Testing Connections to find 'ClubMembership'...
echo.

set INSTANCES=localhost .\SQLEXPRESS (localdb)\MSSQLLocalDB

for %%i in (%INSTANCES%) do (
    echo Testing Instance: %%i
    sqlcmd -S %%i -E -Q "IF EXISTS (SELECT name FROM sys.databases WHERE name = 'ClubMembership') SELECT 'FOUND_DB' ELSE SELECT 'DB_NOT_FOUND'" -t 2 2>nul | findstr "FOUND_DB" > nul
    if errorlevel 1 (
        echo   - Connection failed or Database 'ClubMembership' not found.
    ) else (
        echo   [SUCCESS] Found 'ClubMembership' on instance: %%i
        echo.
        echo   ---------------------------------------------------
        echo   FIX ACTION: Update your appsettings.json to use:
        echo   "GFC": "Server=%%i;Database=ClubMembership;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
        echo   ---------------------------------------------------
        goto :end
    )
)

echo.
echo [!] Could not find 'ClubMembership' automatically. 
echo Please ensure SQL Server is installed and the database is attached.
:end
pause
