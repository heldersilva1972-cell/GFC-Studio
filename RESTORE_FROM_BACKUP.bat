@echo off
set "localdb_instance=(localdb)\MSSQLLocalDB"
set "db_name=ClubMembership"
set "source_bak=C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\ClubMembership_20260103_020639.bak"
set "public_bak=C:\Users\Public\ClubMembership_Restore.bak"

echo ============================================================
echo GFC Database Restore (Fixing Access Denied)
echo ============================================================
echo.

echo 1. Copying backup to Public folder for permissions...
copy /Y "%source_bak%" "%public_bak%"
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Failed to copy backup to Public folder.
    pause
    exit /b
)

echo 2. Granting permissions...
icacls "%public_bak%" /grant Everyone:F >nul

echo 3. Restoring database...
sqlcmd -S "%localdb_instance%" -i "force_restore.sql"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Restore failed. Please try running SSMS as Administrator.
) else (
    echo.
    echo [SUCCESS] Database restored successfully!
)

echo.
pause
