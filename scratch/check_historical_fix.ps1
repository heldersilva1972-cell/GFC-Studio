$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Fetch products
$pCmd = New-Object System.Data.SqlClient.SqlCommand("SELECT Name, Category, RetailPrice, CurrentPrice FROM LiquorItems", $connection)
$pReader = $pCmd.ExecuteReader()
$prices = @{}
while ($pReader.Read()) {
    $name = $pReader["Name"]
    $rp = if ($pReader["RetailPrice"] -ne [DBNull]::Value) { [decimal]$pReader["RetailPrice"] } else { 0 }
    $cp = if ($pReader["CurrentPrice"] -ne [DBNull]::Value) { [decimal]$pReader["CurrentPrice"] } else { 0 }
    $effective = if ($rp -gt 0) { $rp } else { $cp }
    $prices[$name] = $effective
}
$pReader.Close()

# Query all Z-Reports
$zCmd = New-Object System.Data.SqlClient.SqlCommand("SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson, ItemTotalsJson FROM PosZReports ORDER BY Timestamp DESC", $connection)
$zReader = $zCmd.ExecuteReader()

$remainingDiscrepancies = 0
$totalReports = 0

while ($zReader.Read()) {
    $totalReports++
    $id = $zReader["Id"]
    $bartender = $zReader["BartenderName"]
    $ts = $zReader["Timestamp"]
    $gross = [decimal]$zReader["TotalGrossSales"]
    $json = $zReader["SalesSummaryJson"]
    $itemTotalsJson = if ($zReader["ItemTotalsJson"] -ne [DBNull]::Value) { $zReader["ItemTotalsJson"] } else { "{}" }
    
    if ([string]::IsNullOrWhiteSpace($json)) { continue }
    
    try {
        $summary = ConvertFrom-Json $json
    } catch {
        continue
    }

    $itemTotals = @{}
    if ($itemTotalsJson -and $itemTotalsJson -ne "{}" -and $itemTotalsJson -ne "[]") {
        try {
            $parsedTotals = ConvertFrom-Json $itemTotalsJson
            foreach ($p in $parsedTotals.PSObject.Properties) {
                $itemTotals[$p.Name] = [decimal]$p.Value
            }
        } catch {}
    }

    $calcTotal = 0
    foreach ($prop in $summary.PSObject.Properties) {
        $itemName = $prop.Name
        $qty = [int]$prop.Value
        
        if ($itemTotals.ContainsKey($itemName)) {
            $calcTotal += $itemTotals[$itemName]
        } else {
            $cleanKey = $itemName.Replace(" (CREDITED)", "").Replace(" (TOKEN SALE)", "").Replace(" (TOKEN REDEEMED)", "")
            if ($cleanKey.Contains(" (") -and $cleanKey.EndsWith(")")) {
                $openIdx = $cleanKey.IndexOf(" (")
                $cleanKey = $cleanKey.Substring(0, $openIdx).Trim()
            }

            $price = 0
            if ($itemName -like "*(TOKEN REDEEMED)*") {
                $price = 0
            } else {
                if ($prices.ContainsKey($cleanKey)) {
                    $price = $prices[$cleanKey]
                }
            }
            $calcTotal += ($price * $qty)
        }
    }
    
    $diff = [Math]::Abs($gross - $calcTotal)
    if ($diff -gt 0.01) {
        $remainingDiscrepancies++
        Write-Host "Historical Report Discrepancy: ID $id | $ts | $bartender | Stored Gross: $gross | Recalc: $calcTotal | Diff: $($gross - $calcTotal)"
    }
}

Write-Host "------------------------------------------------------------------"
Write-Host "Total Reports Evaluated: $totalReports"
Write-Host "Remaining Historical Discrepancies: $remainingDiscrepancies"

$zReader.Close()
$connection.Close()
