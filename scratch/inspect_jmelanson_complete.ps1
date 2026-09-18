$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=== 1. ALL POS Z-REPORTS IN DB ==="
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT TOP 5 Id, Timestamp, TerminalName, BartenderName, ShiftType, CashTotal, TotalGrossSales, SalesSummaryJson, ItemTotalsJson FROM PosZReports ORDER BY Timestamp DESC"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "ID: $($reader['Id'])"
    Write-Host "Time: $($reader['Timestamp']) | Terminal: $($reader['TerminalName']) | User: $($reader['BartenderName']) | Shift: $($reader['ShiftType'])"
    Write-Host "Gross: $($reader['TotalGrossSales']) | Cash: $($reader['CashTotal'])"
    Write-Host "SalesSummaryJson: $($reader['SalesSummaryJson'])"
    Write-Host "ItemTotalsJson: $($reader['ItemTotalsJson'])"
    Write-Host "----------------------------------------------------"
}
$reader.Close()

Write-Host "`n=== 2. ALL SALES FOR JMELANSON ==="
$cmd2 = $conn.CreateCommand()
$cmd2.CommandText = @"
SELECT 
    Id,
    Timestamp,
    TerminalName,
    BartenderName,
    PaymentType,
    TotalAmount,
    IsVoided,
    ItemsJson
FROM PosSales
WHERE BartenderName LIKE '%Melanson%' OR BartenderName = 'JMelanson'
ORDER BY Timestamp DESC
"@
$reader2 = $cmd2.ExecuteReader()
$jCount = 0
$jGross = 0
$jCash = 0
$jItems = @{}
$jItemTotals = @{}
while ($reader2.Read()) {
    $jCount++
    $amt = [decimal]$reader2['TotalAmount']
    $pay = $reader2['PaymentType']
    $void = $reader2['IsVoided']
    $ts = $reader2['Timestamp']
    $json = $reader2['ItemsJson']

    if (!$void) {
        $jGross += $amt
        if ($pay -eq 'CASH') { $jCash += $amt }
        if (![string]::IsNullOrEmpty($json)) {
            try {
                $items = $json | ConvertFrom-Json
                foreach ($it in $items) {
                    $n = $it.Name
                    $p = [decimal]$it.Price
                    $q = [int]$it.Quantity
                    if (!$jItems.ContainsKey($n)) { $jItems[$n] = 0 }
                    if (!$jItemTotals.ContainsKey($n)) { $jItemTotals[$n] = 0 }
                    $jItems[$n] += $q
                    $jItemTotals[$n] += ($p * $q)
                }
            } catch {}
        }
    }
    Write-Host "Sale [$ts]: Amount: $amt | Pay: $pay | Void: $void | JSON: $json"
}
$reader2.Close()

Write-Host "`n=== JMELANSON SUMMARY ==="
Write-Host "Count: $jCount | Gross: $jGross | Cash: $jCash"
Write-Host "Item Summary: $($jItems | ConvertTo-Json -Compress)"
Write-Host "Item Totals: $($jItemTotals | ConvertTo-Json -Compress)"

$conn.Close()
