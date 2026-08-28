$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# 1. Fetch products
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

# 2. Check all Z-Reports
$zCmd = New-Object System.Data.SqlClient.SqlCommand("SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson FROM PosZReports ORDER BY Timestamp DESC", $connection)
$zReader = $zCmd.ExecuteReader()

$discrepancies = 0

while ($zReader.Read()) {
    $id = $zReader["Id"]
    $bartender = $zReader["BartenderName"]
    $ts = $zReader["Timestamp"]
    $gross = [decimal]$zReader["TotalGrossSales"]
    $json = $zReader["SalesSummaryJson"]
    
    if ([string]::IsNullOrWhiteSpace($json)) { continue }
    
    try {
        $summary = ConvertFrom-Json $json
    } catch {
        continue
    }

    $calcTotal = 0
    foreach ($prop in $summary.PSObject.Properties) {
        $itemName = $prop.Name
        $qty = [int]$prop.Value
        
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
    
    $diff = [Math]::Abs($gross - $calcTotal)
    if ($diff -gt 0.01) {
        $discrepancies++
        Write-Host "DISCREPANCY Found! ZReport ID: $id | Time: $ts | Bartender: $bartender"
        Write-Host "   Stored Gross: $gross | Recalculated Items Total: $calcTotal | Difference: $($gross - $calcTotal)"
        Write-Host "   SalesSummaryJson: $json"
        Write-Host "----------------------------------------------------------------------"
    }
}

Write-Host "Total Z-Reports with discrepancies: $discrepancies"

$zReader.Close()
$connection.Close()
