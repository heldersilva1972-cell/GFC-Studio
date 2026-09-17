$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = @"
SELECT 
    Id,
    Timestamp,
    PaymentType,
    TotalAmount,
    ItemsJson
FROM PosSales
WHERE CAST(Timestamp AS DATE) = '2026-09-16' 
  AND BartenderName = 'MBrancaleone'
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

Write-Host "Found $($salesList.Count) sales for MBrancaleone."

$itemSummary = @{}
$itemTotals = @{}
$grossTotal = 0
$cashTotal = 0
$tokenCredits = 0

foreach ($s in $salesList) {
    if ($s.PaymentType -eq "CASH") {
        $cashTotal += $s.TotalAmount
    }
    if (![string]::IsNullOrEmpty($s.ItemsJson)) {
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
            } elseif ($price -lt 0) {
                $tokenCredits += [Math]::Abs($price * $qty)
            }
        }
    }
}

Write-Host "`nCalculated Shift Totals for MBrancaleone:"
Write-Host "  Gross Sales: $($grossTotal.ToString('C2'))"
Write-Host "  Cash Total:  $($cashTotal.ToString('C2'))"
Write-Host "  Token Credits: $($tokenCredits.ToString('C2'))"

Write-Host "`nItem Summary JSON:"
$salesJson = $itemSummary | ConvertTo-Json -Compress
Write-Host $salesJson

Write-Host "`nItem Totals JSON:"
$totalsJson = $itemTotals | ConvertTo-Json -Compress
Write-Host $totalsJson

$conn.Close()
