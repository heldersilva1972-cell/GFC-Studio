$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "UPDATE dbo.BarSaleEntries SET TotalSales = 346.25 WHERE Id = 3453"
$rows = $cmd.ExecuteNonQuery()
Write-Host "Updated $rows row(s) in BarSaleEntries. Set TotalSales = $346.25 for Id = 3453 (2026-08-28 Night Shift)."
$conn.Close()
