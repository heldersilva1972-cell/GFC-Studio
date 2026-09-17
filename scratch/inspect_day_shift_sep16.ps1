$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=== 1. POS Z-REPORTS FOR 2026-09-16 ==="
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Timestamp, TerminalName, BartenderName, ShiftType, CashTotal, TotalGrossSales, TokenCredits, RecordSalesToBar, PhysicalTokensJson, ShiftDrinkJson, DATALENGTH(SalesSummaryJson) as SalesLen, DATALENGTH(ItemTotalsJson) as ItemsLen, DATALENGTH(InventoryPullsJson) as InvLen FROM PosZReports WHERE CAST(Timestamp AS DATE) = '2026-09-16' ORDER BY Timestamp"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "ID: $($reader['Id']) | Time: $($reader['Timestamp']) | Terminal: $($reader['TerminalName']) | Bartender: $($reader['BartenderName']) | Shift: $($reader['ShiftType']) | Gross: $($reader['TotalGrossSales']) | Cash: $($reader['CashTotal']) | Tokens: $($reader['TokenCredits']) | SalesJsonLen: $($reader['SalesLen'])"
}
$reader.Close()

Write-Host "`n=== 2. POS SALES BREAKDOWN BY BARTENDER & TERMINAL FOR 2026-09-16 ==="
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
WHERE CAST(Timestamp AS DATE) = '2026-09-16' AND (IsVoided = 0 OR IsVoided IS NULL)
GROUP BY TerminalName, BartenderName, PaymentType
ORDER BY Min(Timestamp)
"@
$reader2 = $cmd2.ExecuteReader()
while ($reader2.Read()) {
    Write-Host "Terminal: $($reader2['TerminalName']) | Bartender: $($reader2['BartenderName']) | Pay: $($reader2['PaymentType']) | Count: $($reader2['SaleCount']) | Total: $($reader2['TotalAmount']) | Window: $($reader2['FirstSale']) to $($reader2['LastSale'])"
}
$reader2.Close()

Write-Host "`n=== 3. DAY SHIFT ITEMS SOLD (MBrancaleone) ==="
$cmd3 = $conn.CreateCommand()
$cmd3.CommandText = @"
SELECT 
    ISNULL(j.Category, 'OTHER') AS Category,
    j.ItemName,
    SUM(j.Quantity) AS QuantitySold,
    AVG(j.Price) AS UnitPrice,
    SUM(j.Price * j.Quantity) AS TotalGrossSales
FROM PosSales s
CROSS APPLY OPENJSON(s.ItemsJson) WITH (
    ItemName NVARCHAR(200) '$.Name',
    Price DECIMAL(18,2) '$.Price',
    Quantity INT '$.Quantity',
    Category NVARCHAR(100) '$.Category'
) AS j
WHERE CAST(s.Timestamp AS DATE) = '2026-09-16'
  AND (s.BartenderName = 'MBrancaleone' OR s.Timestamp < '2026-09-16 18:00:00')
  AND (s.IsVoided = 0 OR s.IsVoided IS NULL)
  AND (s.PaymentType IS NULL OR s.PaymentType <> 'PAYOUT')
GROUP BY ISNULL(j.Category, 'OTHER'), j.ItemName
ORDER BY Category, TotalGrossSales DESC
"@
$reader3 = $cmd3.ExecuteReader()
$totalDayGross = 0
while ($reader3.Read()) {
    $cat = $reader3['Category']
    $name = $reader3['ItemName']
    $qty = $reader3['QuantitySold']
    $price = [decimal]$reader3['UnitPrice']
    $tot = [decimal]$reader3['TotalGrossSales']
    $totalDayGross += $tot
    Write-Host ("  [{0,-8}] {1,-30} x {2,2} @ {3,5:C2} = {4,6:C2}" -f $cat, $name, $qty, $price, $tot)
}
$reader3.Close()
Write-Host "Total Day Shift Computed Gross: $($totalDayGross.ToString('C2'))"

Write-Host "`n=== 4. BAR SALE ENTRIES FOR 2026-09-16 ==="
$cmd4 = $conn.CreateCommand()
$cmd4.CommandText = "SELECT Id, SaleDate, Shift, EmployeeUsername, TotalSales, Status, Notes FROM BarSaleEntries WHERE CAST(SaleDate AS DATE) = '2026-09-16'"
$reader4 = $cmd4.ExecuteReader()
while ($reader4.Read()) {
    Write-Host "BarEntry ID: $($reader4['Id']) | Date: $($reader4['SaleDate']) | Shift: $($reader4['Shift']) | User: $($reader4['EmployeeUsername']) | TotalSales: $($reader4['TotalSales']) | Status: $($reader4['Status'])"
}
$reader4.Close()

$conn.Close()
