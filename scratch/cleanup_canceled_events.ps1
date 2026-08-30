$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = @"
-- 1. Unlink sales on deleted events
UPDATE dbo.PosSales SET ActiveEventId = NULL WHERE ActiveEventId IN (SELECT Id FROM dbo.ActiveEvents WHERE IsDeleted = 1);

-- 2. Delete events marked IsDeleted = 1
DELETE FROM dbo.ActiveEvents WHERE IsDeleted = 1;

-- 3. Delete closed events with 0 sales
DELETE FROM dbo.ActiveEvents 
WHERE Status = 1 
  AND Id NOT IN (SELECT DISTINCT ActiveEventId FROM dbo.PosSales WHERE ActiveEventId IS NOT NULL);
"@
$rowsAffected = $cmd.ExecuteNonQuery()
$conn.Close()
Write-Output "Cleaned up rows. Rows affected: $rowsAffected"
