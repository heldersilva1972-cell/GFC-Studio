$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()
$cmd = $connection.CreateCommand()
$cmd.CommandText = "UPDATE dbo.LiquorItems SET RetailPrice = 1.75 WHERE Name = 'Chips'"
$rows = $cmd.ExecuteNonQuery()
Write-Host "Updated $rows row(s) in LiquorItems for Chips."
$connection.Close()
