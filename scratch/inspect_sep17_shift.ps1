$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=== 1. POS Z-REPORTS FOR 2026-09-17 ==="
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Timestamp, TerminalName, BartenderName, ShiftType, CashTotal, TotalGrossSales, TokenCredits, RecordSalesToBar, PhysicalTokensJson, ShiftDrinkJson, DATALENGTH(SalesSummaryJson) as SalesLen, DATALENGTH(ItemTotalsJson) as ItemsLen FROM PosZReports WHERE CAST(Timestamp AS DATE) = '2026-09-17' ORDER BY Timestamp"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "ID: $($reader['Id']) | Time: $($reader['Timestamp']) | Terminal: $($reader['TerminalName']) | Bartender: $($reader['BartenderName']) | Shift: $($reader['ShiftType']) | Gross: $($reader['TotalGrossSales']) | Cash: $($reader['CashTotal']) | Tokens: $($reader['TokenCredits']) | SalesJsonLen: $($reader['SalesLen'])"
}
$reader.Close()

Write-Host "`n=== 2. POS SALES BREAKDOWN FOR 2026-09-17 ==="
$cmd2 = $conn.CreateCommand()
$cmd2.CommandText = @"
SELECT 
    TerminalName,
    BartenderName,
    PaymentType,
    COUNT(*) as SaleCount,
    SUM(TotalAmount) as TotalAmount,
    MIN(Timestamp) as FirstSale,
    MAX(Timestamp) as LastSale
FROM PosSales
WHERE CAST(Timestamp AS DATE) = '2026-09-17' AND (IsVoided = 0 OR IsVoided IS NULL)
GROUP BY TerminalName, BartenderName, PaymentType
ORDER BY Min(Timestamp)
"@
$reader2 = $cmd2.ExecuteReader()
while ($reader2.Read()) {
    Write-Host "Terminal: $($reader2['TerminalName']) | Bartender: $($reader2['BartenderName']) | Pay: $($reader2['PaymentType']) | Count: $($reader2['SaleCount']) | Total: $($reader2['TotalAmount']) | Window: $($reader2['FirstSale']) to $($reader2['LastSale'])"
}
$reader2.Close()

$conn.Close()
