$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandTimeout = 120
$cmd.CommandText = @"
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SELECT 
    Id,
    Timestamp,
    PaymentType,
    TotalAmount,
    ItemsJson
FROM PosSales
WHERE Timestamp > '2026-09-17 17:04:13' AND Timestamp <= '2026-09-17 23:11:24'
  AND (IsVoided = 0 OR IsVoided IS NULL)
ORDER BY Timestamp
"@
$reader = $cmd.ExecuteReader()
$salesList = @()
while ($reader.Read()) {
    $salesList += [PSCustomObject]@{
        Id = $reader['Id']
        Timestamp = $reader['Timestamp']
        PaymentType = $reader['PaymentType']
        TotalAmount = [decimal]$reader['TotalAmount']
        ItemsJson = $reader['ItemsJson']
    }
}
$reader.Close()

Write-Host "Found $($salesList.Count) sales between 5:04 PM and 11:11 PM on Sep 17."

$itemSummary = @{}
$itemTotals = @{}
$grossTotal = 0
$cashTotal = 0

foreach ($s in $salesList) {
    if ($s.PaymentType -eq "CASH") {
        $cashTotal += $s.TotalAmount
    }
    if (![string]::IsNullOrEmpty($s.ItemsJson)) {
        try {
            $items = $s.ItemsJson | ConvertFrom-Json
            foreach ($item in $items) {
                $name = $item.Name
                $price = [decimal]$item.Price
                $qty = [int]$item.Quantity

                if (!$itemSummary.ContainsKey($name)) { $itemSummary[$name] = 0 }
                if (!$itemTotals.ContainsKey($name)) { $itemTotals[$name] = 0 }

                $itemSummary[$name] += $qty
                $itemTotals[$name] += ($price * $qty)

                if ($price -gt 0) {
                    $grossTotal += ($price * $qty)
                }
            }
        } catch {}
    }
}

Write-Host "`nCalculated Night Shift Totals for JMelanson:"
Write-Host "  Gross Sales: $($grossTotal.ToString('C2'))"
Write-Host "  Cash Total:  $($cashTotal.ToString('C2'))"

Write-Host "`nItem Summary JSON:"
$salesJson = $itemSummary | ConvertTo-Json -Compress
Write-Host $salesJson

Write-Host "`nItem Totals JSON:"
$totalsJson = $itemTotals | ConvertTo-Json -Compress
Write-Host $totalsJson

$conn.Close()
