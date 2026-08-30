$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "UPDATE dbo.PosSales SET TotalAmount = 0.00 WHERE ActiveEventId = 50;"
$rows = $cmd.ExecuteNonQuery()
Write-Output "Updated sales: $rows"
$conn.Close()
