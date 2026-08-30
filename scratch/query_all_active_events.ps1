$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Name, Status, IsDeleted, CurrentBalance, InitialAmount, CreatedAt FROM ActiveEvents ORDER BY Id DESC;"
$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$dt = New-Object System.Data.DataTable
$adapter.Fill($dt) | Out-Null
$conn.Close()
Write-Output "Total rows in ActiveEvents: $($dt.Rows.Count)"
$dt | Format-Table -AutoSize | Out-String | Write-Output
