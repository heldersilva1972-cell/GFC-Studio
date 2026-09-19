$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=== All PosSales on 2026-09-17 night shift ==="
$cmd = $conn.CreateCommand()
$cmd.CommandText = @"
SELECT Id, Timestamp, TerminalName, BartenderName, PaymentType, TotalAmount, IsVoided, ActiveEventId, ItemsJson 
FROM PosSales 
WHERE Timestamp > '2026-09-17 17:04:13' AND Timestamp <= '2026-09-17 23:11:24'
ORDER BY Timestamp
"@
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "[$($reader['Timestamp'])] Pay: $($reader['PaymentType']) | Amt: $($reader['TotalAmount']) | Void: $($reader['IsVoided']) | EventId: $($reader['ActiveEventId']) | Items: $($reader['ItemsJson'])"
}
$reader.Close()

$conn.Close()
