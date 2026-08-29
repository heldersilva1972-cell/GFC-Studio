$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "UPDATE dbo.BarSaleEntries SET TotalSales = 197.00 WHERE Id = 3442"
$rows = $cmd.ExecuteNonQuery()
Write-Host "Successfully updated $rows row(s) in BarSaleEntries for Id = 3442 (Aug 26 Night Shift TotalSales set to 197.00)."
$conn.Close()
