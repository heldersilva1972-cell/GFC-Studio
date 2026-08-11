$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Z-Report for Aug 7 night shift
$zId = "7601b530-e787-4e58-9369-d567925f4a7d"
$zQuery = "SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson FROM PosZReports WHERE Id = '$zId'"
$cmd = New-Object System.Data.SqlClient.SqlCommand($zQuery, $connection)
$r = $cmd.ExecuteReader()
$zTime = $null
$term = ""
$salesSummaryJson = ""
$storedGross = 0.0m
if ($r.Read()) {
    $zTime = [DateTime]$r["Timestamp"]
    $term = $r["TerminalName"]
    $storedGross = [decimal]$r["TotalGrossSales"]
    $salesSummaryJson = $r["SalesSummaryJson"].ToString()
}
$r.Close()

# Previous Z-Report time
$prevQuery = "SELECT TOP 1 Timestamp FROM PosZReports WHERE TerminalName = '$term' AND Timestamp < '$($zTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))' ORDER BY Timestamp DESC"
$pcmd = New-Object System.Data.SqlClient.SqlCommand($prevQuery, $connection)
$prevTime = [DateTime]$pcmd.ExecuteScalar()

# Query PosSales in this window
$sQuery = "SELECT Id, Timestamp, BartenderName, PaymentType, TotalAmount, ItemsJson FROM PosSales WHERE TerminalName = '$term' AND Timestamp <= '$($zTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))' AND Timestamp > '$($prevTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))' ORDER BY Timestamp"
$scmd = New-Object System.Data.SqlClient.SqlCommand($sQuery, $connection)
$sr = $scmd.ExecuteReader()

$rawItems = @()
$actualGrossSum = 0.0m

while ($sr.Read()) {
    $amt = [decimal]$sr["TotalAmount"]
    $actualGrossSum += $amt
    $json = $sr["ItemsJson"].ToString()
    if ($json) {
        $items = ConvertFrom-Json $json
        foreach ($i in $items) {
            $rawItems += $i
        }
    }
}
$sr.Close()

# Fetch all LiquorItems from DB
$lcmd = New-Object System.Data.SqlClient.SqlCommand("SELECT Id, Name, Category, RetailPrice, CurrentPrice, ZReportGroup FROM LiquorItems", $connection)
$lr = $lcmd.ExecuteReader()
$dbLiquor = @{}
while ($lr.Read()) {
    $id = $lr["Id"]
    $name = $lr["Name"]
    $cat = $lr["Category"]
    $rp = $lr["RetailPrice"]
    $cp = $lr["CurrentPrice"]
    $zg = $lr["ZReportGroup"]
    $dbLiquor[$name] = [PSCustomObject]@{ Id=$id; Name=$name; Category=$cat; RetailPrice=$rp; CurrentPrice=$cp; ZReportGroup=$zg }
}
$lr.Close()

# Fetch PosTokens from DB
$tcmd = New-Object System.Data.SqlClient.SqlCommand("SELECT Id, Name, Category, SalePrice, CreditValue FROM PosTokens", $connection)
$tr = $tcmd.ExecuteReader()
$dbTokens = @{}
while ($tr.Read()) {
    $name = $tr["Name"]
    $sp = $tr["SalePrice"]
    $cv = $tr["CreditValue"]
    $dbTokens[$name] = [PSCustomObject]@{ Name=$name; SalePrice=$sp; CreditValue=$cv }
}
$tr.Close()

Write-Host "Stored Z-Report Gross Sales: $storedGross"
Write-Host "Actual PosSales Total Sum:   $actualGrossSum"
Write-Host ""
Write-Host "--- ITEM BREAKDOWN FROM ACTUAL POSSALES TRANSACTIONS ---"

# Group actual sales items by Name and Category
$itemGroup = $rawItems | Group-Object -Property Name

$totalLiquorActual = 0.0m
$totalBeerActual = 0.0m
$totalFoodActual = 0.0m
$totalTokenActual = 0.0m
$totalOtherActual = 0.0m

foreach ($g in $itemGroup) {
    $name = $g.Name
    $qty = ($g.Group | Measure-Object -Property Quantity -Sum).Sum
    $sumDollars = ($g.Group | ForEach-Object { $_.Price * $_.Quantity } | Measure-Object -Sum).Sum
    $sampleItem = $g.Group[0]
    $cat = $sampleItem.Category
    $zg = $sampleItem.ZReportGroup

    Write-Host ("{0,-45} | Qty: {1,2} | Cat: {2,-10} | PriceInTx: {3,6:C2} | Total: {4,7:C2}" -f $name, $qty, $cat, $sampleItem.Price, $sumDollars)

    if ($cat -eq "LIQUOR") { $totalLiquorActual += $sumDollars }
    elseif ($cat -eq "BEER") { $totalBeerActual += $sumDollars }
    elseif ($cat -eq "SNACKS" -or $cat -eq "FOOD") { $totalFoodActual += $sumDollars }
    elseif ($cat -eq "TOKENS" -or $cat -eq "TOKEN") { $totalTokenActual += $sumDollars }
    else { $totalOtherActual += $sumDollars }
}

Write-Host "--------------------------------------------------------"
Write-Host "Actual Sales Category Breakdown (from PosSales):"
Write-Host "  Liquor Total: $totalLiquorActual"
Write-Host "  Beer Total:   $totalBeerActual"
Write-Host "  Food Total:   $totalFoodActual"
Write-Host "  Token Total:  $totalTokenActual"
Write-Host "  Other Total:  $totalOtherActual"
Write-Host "  SUM TOTAL:    $($totalLiquorActual + $totalBeerActual + $totalFoodActual + $totalTokenActual + $totalOtherActual)"

$connection.Close()
