$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandTimeout = 30
$cmd.CommandText = "DELETE FROM dbo.ActiveEvents WHERE Id = 43 OR Id = 42 OR IsDeleted = 1;"
$rowsAffected = $cmd.ExecuteNonQuery()
$conn.Close()
Write-Output "Deleted rows: $rowsAffected"
