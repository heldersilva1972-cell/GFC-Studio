@echo off
set "localdb_instance=(localdb)\MSSQLLocalDB"
set "db_name=ClubMembership"
set "output_file=GFC_DATABASE_INVENTORY.txt"

echo ============================================================
echo GFC Database Field Inventory Generator
echo ============================================================
echo Connecting to: %localdb_instance%
echo Target Database: %db_name%
echo.
echo Generating list of all tables and fields...

sqlcmd -S "%localdb_instance%" -d %db_name% -Q "SET NOCOUNT ON; SELECT LEFT(t.name, 30) AS [Table], LEFT(c.name, 30) AS [Field], LEFT(ty.name, 15) AS [Type], CASE WHEN c.is_nullable = 1 THEN 'Null' ELSE 'Req' END AS [Nullable] FROM sys.columns c JOIN sys.tables t ON c.object_id = t.object_id JOIN sys.types ty ON c.user_type_id = ty.user_type_id WHERE t.is_ms_shipped = 0 ORDER BY t.name, c.column_id;" -W -s "," > "%output_file%"

echo.
echo ============================================================
echo Inventory Complete! 
echo Results saved to: %output_file%
echo ============================================================
pause
