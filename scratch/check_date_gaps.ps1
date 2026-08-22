$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Check all dates in 2021
$query = "SELECT WeekEndingDate FROM LotteryWeeklyStats WHERE YEAR(WeekEndingDate) = 2021 ORDER BY WeekEndingDate"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

Write-Host "2021 WeekEndingDate Listing:"
Write-Host "Date        | Day of Week"
Write-Host "-------------------------"
while ($r.Read()) {
    $d = [DateTime]$r["WeekEndingDate"]
    Write-Host ("{0:yyyy-MM-dd} | {1}" -f $d, $d.DayOfWeek)
}
$r.Close()

# Check gap between consecutive dates in 2021
$queryGaps = "SELECT WeekEndingDate FROM LotteryWeeklyStats WHERE YEAR(WeekEndingDate) = 2021 ORDER BY WeekEndingDate"
$cmdG = New-Object System.Data.SqlClient.SqlCommand($queryGaps, $connection)
$rG = $cmdG.ExecuteReader()
$dates = @()
while ($rG.Read()) { $dates += [DateTime]$rG["WeekEndingDate"] }
$rG.Close()

Write-Host "`nGap Analysis for 2021 Records:"
Write-Host "-------------------------------"
for ($i = 0; $i -lt ($dates.Count - 1); $i++) {
    $diff = ($dates[$i+1] - $dates[$i]).Days
    if ($diff -ne 7) {
        Write-Host ("GAP DETECTED: Between {0:yyyy-MM-dd} and {1:yyyy-MM-dd} ({2} days diff!)" -f $dates[$i], $dates[$i+1], $diff)
    }
}

$connection.Close()
