$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = @"
UPDATE dbo.PosSales
SET TotalAmount = 0.00,
    ItemsJson = REPLACE(REPLACE(ItemsJson, '"Price":2.5', '"Price":0.0'), '"Price":2.50', '"Price":0.00')
WHERE TotalAmount = 10.00 AND (ItemsJson LIKE '%Budweiser%' OR ItemsJson LIKE '%Coors%');
"@
$rows = $cmd.ExecuteNonQuery()
Write-Output "Updated transaction rows to $0.00: $rows"

$conn.Close()
