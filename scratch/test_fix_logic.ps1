$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Z-Report for Aug 7 night shift
$zId = "7601b530-e787-4e58-9369-d567925f4a7d"
$zQuery = "SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson FROM PosZReports WHERE Id = '$zId'"
$cmd = New-Object System.Data.SqlClient.SqlCommand($zQuery, $connection)
$r = $cmd.ExecuteReader()
$r.Read()
$zTime = [DateTime]$r["Timestamp"]
$term = $r["TerminalName"]
$bart = $r["BartenderName"]
$gross = [decimal]$r["TotalGrossSales"]
$cash = [decimal]$r["CashTotal"]
$salesSummaryJson = $r["SalesSummaryJson"].ToString()
$r.Close()

# Fetch LiquorItems
$lcmd = New-Object System.Data.SqlClient.SqlCommand("SELECT Name, Category, RetailPrice, CurrentPrice, ZReportGroup FROM LiquorItems", $connection)
$lr = $lcmd.ExecuteReader()
$items = @()
while ($lr.Read()) {
    $items += [PSCustomObject]@{
        Name = $lr["Name"].ToString()
        Category = if ($lr["Category"] -ne [DBNull]::Value) { $lr["Category"].ToString() } else { $null }
        RetailPrice = if ($lr["RetailPrice"] -ne [DBNull]::Value) { [decimal]$lr["RetailPrice"] } else { 0.0 }
        CurrentPrice = if ($lr["CurrentPrice"] -ne [DBNull]::Value) { [decimal]$lr["CurrentPrice"] } else { 0.0 }
        ZReportGroup = if ($lr["ZReportGroup"] -ne [DBNull]::Value) { [int]$lr["ZReportGroup"] } else { 0 }
    }
}
$lr.Close()

# Fetch PosTokens
$tcmd = New-Object System.Data.SqlClient.SqlCommand("SELECT Name, SalePrice, CreditValue FROM PosTokens", $connection)
$tr = $tcmd.ExecuteReader()
$tokens = @()
while ($tr.Read()) {
    $tokens += [PSCustomObject]@{
        Name = $tr["Name"].ToString()
        SalePrice = [decimal]$tr["SalePrice"]
        CreditValue = if ($tr["CreditValue"] -ne [DBNull]::Value) { [decimal]$tr["CreditValue"] } else { 0.0 }
    }
}
$tr.Close()

$connection.Close()

$summaryDict = ConvertFrom-Json $salesSummaryJson

$regularSalesSummary = [ordered]@{}
foreach ($prop in $summaryDict.PSObject.Properties) {
    if ($prop.Name -ne "NO SALE (Drawer Open)" -and -not $prop.Name.StartsWith("PAYOUT:")) {
        $regularSalesSummary[$prop.Name] = [int]$prop.Value
    }
}

Write-Host "`n--- TESTING PROPOSED FIX FOR PRICE LOOKUP ---"
$foodSnackTotal = [decimal]0.0
$liquorTotal = [decimal]0.0

foreach ($key in $regularSalesSummary.Keys) {
    $qty = $regularSalesSummary[$key]
    $cleanKey = $key
    $cleanKey = $cleanKey.Replace(" (CREDITED)", "").Replace(" (TOKEN SALE)", "").Replace(" (TOKEN REDEEMED)", "")

    $prod = $items | Where-Object { $_.Name -and $_.Name.Equals($cleanKey, [System.StringComparison]::OrdinalIgnoreCase) } | Select-Object -First 1
    if (-not $prod -and $cleanKey.Contains(" (") -and $cleanKey.EndsWith(")")) {
        $openIdx = $cleanKey.IndexOf(" (")
        $innerName = $cleanKey.Substring($openIdx + 2, $cleanKey.Length - $openIdx - 3).Trim()
        $prod = $items | Where-Object { $_.Name -and $_.Name.Equals($innerName, [System.StringComparison]::OrdinalIgnoreCase) } | Select-Object -First 1
    }

    $category = if ($prod) { $prod.Category } else { $null }
    if ($key.Contains("(DARTS")) { $category = "DARTS ROUND" }
    elseif ($key.StartsWith("TAB DEPOSIT:")) { $category = "DEPOSITS" }
    elseif (-not $category -and ($key.Contains("TOKEN CREDIT") -or $key.Contains("(TOKEN REDEEMED)") -or $key.Contains("TOKEN"))) { $category = "TOKENS" }

    $price = [decimal]0.0
    if ($key.Contains("(TOKEN REDEEMED)") -or $key.Contains("TOKEN REDEEMED")) {
        $price = [decimal]0.0
    } else {
        # FIX LOGIC: Prioritize product's price if product match is found
        if ($prod) {
            $price = if ($prod.RetailPrice -gt 0.0) { [decimal]$prod.RetailPrice } else { [decimal]$prod.CurrentPrice }
        }
        
        # If product price is 0 and it's a token sale, fallback to token generic sale price
        if ($price -eq 0.0 -and ($key.Contains("TOKEN") -or $key.Contains("Token"))) {
            $tokenMatch = $tokens | Sort-Object { $_.Name.Length } -Descending | Where-Object { $cleanKey.StartsWith($_.Name, [System.StringComparison]::OrdinalIgnoreCase) -or $key.StartsWith($_.Name, [System.StringComparison]::OrdinalIgnoreCase) } | Select-Object -First 1
            if ($tokenMatch) { $price = [decimal]$tokenMatch.SalePrice }
        }
    }

    $lineTotal = [decimal]($price * $qty)
    $zg = if ($prod) { $prod.ZReportGroup } else { 0 }
    $catUpper = if ($category) { $category.ToUpper() } else { "MISC" }
    $nameLower = $key.ToLower()

    # Category summary grouping logic
    $assignedGroup = ""
    if ($zg -eq 1) { $foodSnackTotal += $lineTotal; $assignedGroup = "FOOD/SNACK (ZGroup 1)" }
    elseif ($zg -eq 2) { $liquorTotal += $lineTotal; $assignedGroup = "LIQUOR (ZGroup 2)" }
    elseif ($zg -eq 3) { $assignedGroup = "EXCLUDED (ZGroup 3)" }
    else {
        if ($catUpper -eq "FOOD" -or $catUpper -eq "CANDY" -or $catUpper -eq "SNACK" -or $catUpper -eq "SNACKS" -or $catUpper -eq "NON-ALCOHOLIC" -or $catUpper -eq "BEVERAGE" -or $catUpper -eq "BEVERAGES" -or $catUpper -eq "SODA" -or $catUpper -eq "WATER" -or $nameLower.Contains("soda") -or $nameLower.Contains("water") -or $nameLower.Contains("redbull") -or $nameLower.Contains("red bull")) {
            $foodSnackTotal += $lineTotal
            $assignedGroup = "FOOD/SNACK (Fallback)"
        } else {
            $liquorTotal += $lineTotal
            $assignedGroup = "LIQUOR (Fallback)"
        }
    }

    Write-Host ("{0,-45} | Qty: {1,2} | FixedPrice: {2,6:C2} | LineTotal: {3,7:C2} | Assigned: {4}" -f $key, $qty, $price, $lineTotal, $assignedGroup)
}

Write-Host "--------------------------------------------------------------------------"
Write-Host ("Fixed FOOD & SNACKS TOTAL:   {0:C2}" -f $foodSnackTotal)
Write-Host ("Fixed LIQUOR & DRINKS TOTAL: {0:C2}" -f $liquorTotal)
Write-Host ("Sum of Category Summaries:   {0:C2}" -f ($foodSnackTotal + $liquorTotal))
Write-Host ("Stored Total Gross Sales:    {0:C2}" -f $gross)
