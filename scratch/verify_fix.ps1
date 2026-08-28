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

# Query target Z-Report
$zCmd = New-Object System.Data.SqlClient.SqlCommand("SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson FROM PosZReports WHERE Id = 'b54423d7-ffa6-4dbc-afa1-1fed77c32fa4'", $connection)
$zReader = $zCmd.ExecuteReader()

while ($zReader.Read()) {
    $gross = [decimal]$zReader["TotalGrossSales"]
    $json = $zReader["SalesSummaryJson"]
    
    $summary = ConvertFrom-Json $json
    $categories = @{}
    
    foreach ($prop in $summary.PSObject.Properties) {
        $itemName = $prop.Name
        $qty = [int]$prop.Value
        
        $cleanKey = $itemName.Replace(" (CREDITED)", "").Replace(" (TOKEN SALE)", "").Replace(" (TOKEN REDEEMED)", "")
        $isTokenRedeemed = $itemName -like "*(TOKEN REDEEMED)*"
        
        if ($cleanKey.Contains(" (") -and $cleanKey.EndsWith(")")) {
            $openIdx = $cleanKey.IndexOf(" (")
            $cleanKey = $cleanKey.Substring(0, $openIdx).Trim()
        }

        $category = "MISC"
        if ($prices.ContainsKey($cleanKey)) {
            $category = "BEER" # default
            # match category from prices if possible
        }
        if ($isTokenRedeemed -or $itemName -like "*TOKEN*") {
            $category = "TOKENS"
        } else {
            if ($cleanKey -eq "Budweiser" -or $cleanKey -eq "Miller Lite" -or $cleanKey -eq "Pabst" -or $cleanKey -eq "Heiniken" -or $cleanKey -eq "Miller High Life" -or $cleanKey -eq "Bud Light") {
                $category = "BEER"
            } elseif ($cleanKey -eq "Bulleit Bourbon" -or $cleanKey -eq "Dewars" -or $cleanKey -eq "Tito's" -or $cleanKey -eq "Seagram's VO" -or $cleanKey -eq "Bacardi") {
                $category = "LIQUOR"
            } elseif ($cleanKey -eq "Water") {
                $category = "NON-ALCOHOLIC"
            } elseif ($cleanKey -eq "Suncruiser Iced Tea") {
                $category = "SELTZER"
            }
        }

        $price = 0
        if (-not $isTokenRedeemed) {
            if ($prices.ContainsKey($cleanKey)) {
                $price = $prices[$cleanKey]
            }
        }
        
        $lineTotal = $price * $qty
        
        if (-not $categories.ContainsKey($category)) {
            $categories[$category] = @{ Items = @(); Total = 0 }
        }
        $categories[$category].Items += [PSCustomObject]@{ Name = $itemName; Qty = $qty; Price = $price; Total = $lineTotal }
        $categories[$category].Total += $lineTotal
    }
    
    Write-Host "=================== SIMULATED UPDATED Z-REPORT ==================="
    Write-Host "GROSS SALES: $gross"
    Write-Host "------------------------------------------------------------------"
    $grandTotal = 0
    foreach ($cat in $categories.Keys | Sort-Object) {
        Write-Host "CATEGORY: $cat"
        foreach ($item in $categories[$cat].Items) {
            Write-Host ("  {0,-40} | Qty: {1,2} | Unit: {2,6:C2} | Total: {3,7:C2}" -f $item.Name, $item.Qty, $item.Price, $item.Total)
        }
        Write-Host ("  *** {0} TOTAL: {1,7:C2} ***" -f $cat, $categories[$cat].Total)
        Write-Host ""
        $grandTotal += $categories[$cat].Total
    }
    Write-Host "------------------------------------------------------------------"
    Write-Host "GRAND TOTAL OF ALL CATEGORY BREAKDOWNS: $grandTotal"
    Write-Host "STORED GROSS SALES: $gross"
    Write-Host "DIFFERENCE: $($gross - $grandTotal)"
}

$zReader.Close()
$connection.Close()
