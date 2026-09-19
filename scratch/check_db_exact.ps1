$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=================== 1. PosZReports (2026-09-17+) ==================="
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT * FROM PosZReports WHERE Timestamp >= '2026-09-17' ORDER BY Timestamp DESC"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "----------------------------------------------------"
    for ($i = 0; $i -lt $reader.FieldCount; $i++) {
        Write-Host "$($reader.GetName($i)): $($reader.GetValue($i))"
    }
}
$reader.Close()

Write-Host "`n=================== 2. BarSaleEntries (2026-09-17) ==================="
$cmd2 = $conn.CreateCommand()
$cmd2.CommandText = "SELECT * FROM BarSaleEntries WHERE SaleDate = '2026-09-17' OR AdjustedSaleDate = '2026-09-17'"
$reader2 = $cmd2.ExecuteReader()
while ($reader2.Read()) {
    Write-Host "----------------------------------------------------"
    for ($i = 0; $i -lt $reader2.FieldCount; $i++) {
        Write-Host "$($reader2.GetName($i)): $($reader2.GetValue($i))"
    }
}
$reader2.Close()

Write-Host "`n=================== 3. PosSales summary for JMelanson ==================="
$cmd3 = $conn.CreateCommand()
$cmd3.CommandText = "SELECT BartenderName, COUNT(*) as Cnt, SUM(TotalAmount) as TotalGross, SUM(CASE WHEN PaymentType = 'CASH' THEN TotalAmount ELSE 0 END) as CashGross FROM PosSales WHERE Timestamp >= '2026-09-17 17:00:00' AND (IsVoided = 0 OR IsVoided IS NULL) GROUP BY BartenderName"
$reader3 = $cmd3.ExecuteReader()
while ($reader3.Read()) {
    Write-Host "Bartender: $($reader3['BartenderName']) | Count: $($reader3['Cnt']) | TotalGross: $($reader3['TotalGross']) | CashGross: $($reader3['CashGross'])"
}
$reader3.Close()

$conn.Close()
