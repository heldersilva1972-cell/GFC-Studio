$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT 
    COUNT(*) as TotalWeeks,
    MIN(WeekEndingDate) as EarliestDate,
    MAX(WeekEndingDate) as LatestDate
FROM LotteryWeeklyStats
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

if ($r.Read()) {
    Write-Host ("OVERALL RANGE: {0:yyyy-MM-dd} to {1:yyyy-MM-dd} | Total Recorded Weeks: {2}" -f $r["EarliestDate"], $r["LatestDate"], $r["TotalWeeks"])
}
$r.Close()

$queryYear = @"
SELECT 
    YEAR(WeekEndingDate) as Yr,
    COUNT(*) as WeeksCount,
    MIN(WeekEndingDate) as MinDate,
    MAX(WeekEndingDate) as MaxDate
FROM LotteryWeeklyStats
GROUP BY YEAR(WeekEndingDate)
ORDER BY Yr DESC
"@

$cmdY = New-Object System.Data.SqlClient.SqlCommand($queryYear, $connection)
$rY = $cmdY.ExecuteReader()

Write-Host "`nBreakdown by Year:"
Write-Host "Year | Weeks Recorded | Date Range"
Write-Host "--------------------------------------------------------"
while ($rY.Read()) {
    Write-Host ("{0,4} | {1,14} | {2:yyyy-MM-dd} to {3:yyyy-MM-dd}" -f $rY["Yr"], $rY["WeeksCount"], $rY["MinDate"], $rY["MaxDate"])
}
$rY.Close()
$connection.Close()
