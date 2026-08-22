$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT 
    COUNT(*) as TotalRows,
    SUM(CASE WHEN OnlineNetSales <> 0 THEN 1 ELSE 0 END) as HasOnlineSales,
    SUM(CASE WHEN OnlineCommission <> 0 THEN 1 ELSE 0 END) as HasOnlineComm,
    SUM(CASE WHEN OnlineCashes <> 0 THEN 1 ELSE 0 END) as HasOnlineCashes,
    SUM(CASE WHEN InstantGrossSales <> 0 THEN 1 ELSE 0 END) as HasInstantSales,
    SUM(CASE WHEN InstantCommission <> 0 THEN 1 ELSE 0 END) as HasInstantComm,
    SUM(CASE WHEN InstantCashes <> 0 THEN 1 ELSE 0 END) as HasInstantCashes,
    SUM(CASE WHEN (OnlineCashBonus <> 0 OR InstantCashBonus <> 0) THEN 1 ELSE 0 END) as HasCashBonus,
    SUM(CASE WHEN (OnlineClaimsBonus <> 0 OR InstantClaimsBonus <> 0) THEN 1 ELSE 0 END) as HasClaimsBonus,
    SUM(CASE WHEN TotalDue <> 0 THEN 1 ELSE 0 END) as HasTotalDue
FROM LotteryWeeklyStats
WHERE WeekEndingDate >= '2021-01-01' AND WeekEndingDate <= '2025-12-31'
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

if ($r.Read()) {
    Write-Host "Data Completeness Analysis (240 Records from 2021 to 2025):"
    Write-Host "---------------------------------------------------------"
    Write-Host "Total Records: " $r["TotalRows"]
    Write-Host "Online Net Sales Populated: " $r["HasOnlineSales"] "/" $r["TotalRows"]
    Write-Host "Online Commission Populated: " $r["HasOnlineComm"] "/" $r["TotalRows"]
    Write-Host "Online Cashes Populated:     " $r["HasOnlineCashes"] "/" $r["TotalRows"]
    Write-Host "Instant Sales Populated:    " $r["HasInstantSales"] "/" $r["TotalRows"]
    Write-Host "Instant Commission Populated:" $r["HasInstantComm"] "/" $r["TotalRows"]
    Write-Host "Instant Cashes Populated:    " $r["HasInstantCashes"] "/" $r["TotalRows"]
    Write-Host "Cash Bonuses Populated:      " $r["HasCashBonus"] "/" $r["TotalRows"]
    Write-Host "Claims Bonuses Populated:    " $r["HasClaimsBonus"] "/" $r["TotalRows"]
    Write-Host "Total Due Populated:         " $r["HasTotalDue"] "/" $r["TotalRows"]
}
$r.Close()

# Check if any columns contain NULL values
$nullQuery = @"
SELECT 
    SUM(CASE WHEN OnlineNetSales IS NULL THEN 1 ELSE 0 END) as NullOLSales,
    SUM(CASE WHEN OnlineCommission IS NULL THEN 1 ELSE 0 END) as NullOLComm,
    SUM(CASE WHEN InstantGrossSales IS NULL THEN 1 ELSE 0 END) as NullInstSales,
    SUM(CASE WHEN InstantCommission IS NULL THEN 1 ELSE 0 END) as NullInstComm,
    SUM(CASE WHEN TotalDue IS NULL THEN 1 ELSE 0 END) as NullTotalDue
FROM LotteryWeeklyStats
WHERE WeekEndingDate >= '2021-01-01' AND WeekEndingDate <= '2025-12-31'
"@
$cmdNull = New-Object System.Data.SqlClient.SqlCommand($nullQuery, $connection)
$rNull = $cmdNull.ExecuteReader()
if ($rNull.Read()) {
    Write-Host "`nNULL Check:"
    Write-Host "NULL Online Sales: " $rNull["NullOLSales"]
    Write-Host "NULL Online Comm:  " $rNull["NullOLComm"]
    Write-Host "NULL Instant Sales:" $rNull["NullInstSales"]
    Write-Host "NULL Instant Comm: " $rNull["NullInstComm"]
    Write-Host "NULL Total Due:    " $rNull["NullTotalDue"]
}
$rNull.Close()
$connection.Close()
