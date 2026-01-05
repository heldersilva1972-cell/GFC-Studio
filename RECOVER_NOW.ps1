$bakPath = "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\ClubMembership_20260103_020639.bak"
$publicPath = "C:\Users\Public\ClubMembership_Restore.bak"
$dbName = "ClubMembership"
$instance = "(localdb)\MSSQLLocalDB"

Write-Host "--- GFC Database Recovery Protocol ---" -ForegroundColor Cyan

# 1. Close current connections / Restart instance
Write-Host "Step 1: Restarting SQL Instance..."
sqllocaldb stop mssqllocaldb -k
sqllocaldb start mssqllocaldb

# 2. Prepare Backup File
Write-Host "Step 2: Preparing backup file in Public folder..."
Copy-Item $bakPath $publicPath -Force
icacls $publicPath /grant "Everyone:F" | Out-Null

# 3. Get Logical File Names
Write-Host "Step 3: Reading backup metadata..."
$fileList = sqlcmd -S $instance -Q "RESTORE FILELISTONLY FROM DISK = '$publicPath'" -W
Write-Host $fileList

# Extract logical names (usually the first two lines of the data portion)
# We will use a more robust restore that moves files to the correct GFC directory
$mdfLogical = "ClubMembership"
$ldfLogical = "ClubMembership_log"

# 4. Perform Restore
Write-Host "Step 4: Executing Recovery Restore..."
$restoreSql = @"
USE master;
ALTER DATABASE [$dbName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE [$dbName] FROM DISK = '$publicPath' 
WITH REPLACE, 
MOVE '$mdfLogical' TO 'C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\ClubMembership.mdf',
MOVE '$ldfLogical' TO 'C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\ClubMembership_log.ldf';
ALTER DATABASE [$dbName] SET MULTI_USER;
"@

sqlcmd -S $instance -Q $restoreSql

Write-Host "Step 5: Verifying data..."
sqlcmd -S $instance -d $dbName -Q "SELECT TOP 5 Name, Email, Status FROM Members"

Write-Host "--- Recovery Complete ---" -ForegroundColor Green
