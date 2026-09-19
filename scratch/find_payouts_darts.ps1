$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=== All Payouts or Darts sales on 2026-09-17 ==="
$cmd = $conn.CreateCommand()
$cmd.CommandText = @"
SELECT Id, Timestamp, TerminalName, BartenderName, PaymentType, TotalAmount, IsVoided, ItemsJson 
FROM PosSales 
WHERE Timestamp >= '2026-09-17 00:00:00' AND Timestamp <= '2026-09-18 04:00:00'
  AND (PaymentType LIKE '%PAYOUT%' OR ItemsJson LIKE '%PAYOUT%' OR ItemsJson LIKE '%DART%' OR PaymentType LIKE '%DART%')
ORDER BY Timestamp
"@
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "[$($reader['Timestamp'])] Pay: $($reader['PaymentType']) | Amt: $($reader['TotalAmount']) | Items: $($reader['ItemsJson'])"
}
$reader.Close()

$conn.Close()
