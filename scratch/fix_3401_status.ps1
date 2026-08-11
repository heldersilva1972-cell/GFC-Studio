$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$cmd = $connection.CreateCommand()
$cmd.CommandText = "UPDATE dbo.BarSaleEntries SET Status = 'Submitted' WHERE Id = 3401"
$rows = $cmd.ExecuteNonQuery()
Write-Host "Updated $rows row(s) in BarSaleEntries setting Status = 'Submitted' for Id = 3401."

$connection.Close()
