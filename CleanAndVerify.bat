@echo off
echo ========================================
echo STEP 1: Cleaning Database
echo ========================================
sqlcmd -S ".\SQLEXPRESS" -d "ClubMembership" -E -i "ForceFreshSync.sql"

echo.
echo ========================================
echo STEP 2: Verification
echo ========================================
sqlcmd -S ".\SQLEXPRESS" -d "ClubMembership" -E -Q "SELECT COUNT(*) as LastIndexCount FROM ControllerLastIndexes; SELECT COUNT(*) as EventCount FROM ControllerEvents;"

echo.
echo ========================================
echo Database is clean. 
echo NOW RESTART THE APPLICATION.
echo Then check logs for:
echo   - "Controller 223213880 reports TotalEvents = 150939168"
echo   - "Gap=150939168"
echo   - "Skipping to last 1000"
echo ========================================
pause
