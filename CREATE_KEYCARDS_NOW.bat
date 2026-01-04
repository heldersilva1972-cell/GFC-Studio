@echo off
echo ========================================
echo Creating KeyCards Table (LocalDB)
echo ========================================
echo.

REM Using LocalDB instead of SQL Server Express
sqlcmd -S "(localdb)\MSSQLLocalDB" -d ClubMembership -i "docs\DatabaseScripts\CREATE_KEYCARDS_TABLE.sql"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo SUCCESS! KeyCards table created.
    echo ========================================
    echo.
    echo NEXT STEP: Restart your GFC application
    echo.
) else (
    echo.
    echo ========================================
    echo ERROR: Failed to create table
    echo ========================================
    echo.
    echo Possible issues:
    echo 1. LocalDB not started - run: sqllocaldb start MSSQLLocalDB
    echo 2. Database doesn't exist
    echo 3. Try opening the SQL file in SSMS instead
    echo.
)

pause
