# 1. Kill the web server to release file locks
Write-Host "Stopping any running functionality..." -ForegroundColor Yellow
taskkill /F /IM "GFC.BlazorServer.exe" /T 2>$null
taskkill /F /IM "dotnet.exe" /T 2>$null

# 2. Kill existing SQL connections to force restore
Write-Host "Killing active SQL connections..." -ForegroundColor Yellow
$killSql = "
DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('ClubMembership')
EXEC(@kill);
"
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q $killSql 2>$null

# 3. Perform the Restore
Write-Host "Restoring Production Data..." -ForegroundColor Cyan
$restoreCmd = "RESTORE DATABASE [ClubMembership] FROM DISK = N'C:\Users\Public\Documents\ClubMembership_Production.bak' WITH FILE = 1, REPLACE, RECOVERY;"
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q $restoreCmd

# 4. Verify results
Write-Host "`nVerifying Data..." -ForegroundColor Green
sqlcmd -S "(localdb)\MSSQLLocalDB" -d ClubMembership -Q "SELECT TOP 5 Id, FirstName, LastName FROM Members"
