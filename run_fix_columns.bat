@echo off
sqlcmd -S . -d ClubMembership -i "docs\DatabaseScripts\Fix_SystemSettings_Operations_Columns.sql" > fix_output.txt 2>&1
if %ERRORLEVEL% EQU 0 (
    echo Database fix applied successfully.
) else (
    echo Database fix failed. Check fix_output.txt
)
type fix_output.txt
