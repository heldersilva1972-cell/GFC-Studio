@echo off
echo ========================================
echo Creating Admin User
echo ========================================
echo.

sqlcmd -S "(localdb)\MSSQLLocalDB" -d ClubMembership -i "docs\DatabaseScripts\CREATE_ADMIN_USER.sql"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo SUCCESS! Admin user created.
    echo ========================================
    echo.
    echo Login Credentials:
    echo   Username: admin
    echo   Password: Admin123!
    echo.
    echo IMPORTANT: Change this password after first login!
    echo.
) else (
    echo.
    echo ========================================
    echo ERROR: Failed to create admin user
    echo ========================================
    echo.
    echo Try opening the SQL file in SSMS instead:
    echo docs\DatabaseScripts\CREATE_ADMIN_USER.sql
    echo.
)

pause
