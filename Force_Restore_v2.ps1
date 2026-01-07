# 1. Stop any running apps to release locks
Write-Host "Stopping GFC App..." -ForegroundColor Yellow
taskkill /F /IM "GFC.BlazorServer.exe" /T 2>$null
taskkill /F /IM "dotnet.exe" /T 2>$null

# 2. Create the robust SQL script
$sqlContent = @"
USE [master];
GO
-- Kill active connections to ClubMembership
DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('ClubMembership')
EXEC(@kill);
GO

-- Restore Database
RESTORE DATABASE [ClubMembership] 
FROM DISK = N'C:\Users\Public\Documents\ClubMembership_Production.bak' 
WITH FILE = 1, REPLACE, RECOVERY;
GO

-- Verify
USE [ClubMembership];
GO
SELECT TOP 5 Id, FirstName, LastName, Email FROM Members;
GO
"@

# 3. Write to a temp file
$tempFile = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\temp_restore_script.sql"
Set-Content -Path $tempFile -Value $sqlContent

# 4. Execute using -i (Input File) to avoid quoting errors
Write-Host "Restoring Production Data..." -ForegroundColor Cyan
sqlcmd -S "(localdb)\MSSQLLocalDB" -i $tempFile

# 5. Cleanup
Remove-Item $tempFile
Write-Host "`nDONE. Please restart your app." -ForegroundColor Green
