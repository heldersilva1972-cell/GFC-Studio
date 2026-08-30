$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, ActiveEventId, ItemsJson, TotalAmount, PaymentType, Timestamp, BartenderName, TerminalName FROM PosSales ORDER BY Timestamp DESC"
$dt = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dt) | Out-Null
Write-Output "=== Recent PosSales ==="
foreach ($row in $dt.Rows | Select-Object -First 5) {
    Write-Output "Id: $($row['Id']), ActiveEventId: $($row['ActiveEventId']), TotalAmount: $($row['TotalAmount']), PaymentType: $($row['PaymentType']), Timestamp: $($row['Timestamp']), Items: $($row['ItemsJson'])"
}
$conn.Close()
