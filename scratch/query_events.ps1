$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Timestamp, ActiveEventId, TotalAmount, PaymentType FROM PosSales WHERE ActiveEventId = 42;"
$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$dt = New-Object System.Data.DataTable
$adapter.Fill($dt) | Out-Null
$conn.Close()
Write-Output "Sales count for Event 42: $($dt.Rows.Count)"
$dt | Format-Table -AutoSize | Out-String | Write-Output
