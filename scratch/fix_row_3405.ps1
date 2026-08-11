$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$cmd = $connection.CreateCommand()
$cmd.CommandText = "UPDATE dbo.BarSaleEntries SET TotalSales = 251.00 WHERE Id = 3405"
$rows = $cmd.ExecuteNonQuery()
Write-Host "Updated $rows row(s) in BarSaleEntries for Aug 10 Night Shift (set TotalSales = 251.00)."

$connection.Close()
