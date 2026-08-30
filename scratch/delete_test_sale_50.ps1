$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "DELETE FROM dbo.PosSales WHERE ActiveEventId = 50;"
$rows = $cmd.ExecuteNonQuery()
Write-Output "Deleted leftover test sales for Event 50: $rows"
$conn.Close()
