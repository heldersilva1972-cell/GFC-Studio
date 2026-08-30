$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Name, CurrentBalance, Enable100PercentDonatedProceeds FROM ActiveEvents WHERE Id = 50; SELECT Id, ActiveEventId, TotalAmount FROM PosSales WHERE ActiveEventId = 50;"
$da = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$ds = New-Object System.Data.DataSet
$da.Fill($ds) | Out-Null
$ds.Tables[0] | Format-Table | Out-String | Write-Output
$ds.Tables[1] | Format-Table | Out-String | Write-Output
$conn.Close()
