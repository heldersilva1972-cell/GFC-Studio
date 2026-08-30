$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, ActiveEventId, ItemsJson, TotalAmount, PaymentType, Timestamp, BartenderName, TerminalName FROM PosSales WHERE ActiveEventId IN (50, 51) OR Timestamp >= DATEADD(hour, -2, GETUTCDATE()) ORDER BY Id DESC"
$dt = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dt) | Out-Null
Write-Output "=== PosSales (Total: $($dt.Rows.Count)) ==="
$dt | Format-Table -AutoSize | Out-String | Write-Output

foreach ($row in $dt.Rows) {
    Write-Output "Sale Id: $($row['Id']), ActiveEventId: $($row['ActiveEventId']), ItemsJson: $($row['ItemsJson'])"
}

$conn.Close()
