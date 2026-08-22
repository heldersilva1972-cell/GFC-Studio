$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Fetch all existing WeekEndingDates between 2021 and 2025
$query = "SELECT WeekEndingDate FROM LotteryWeeklyStats WHERE WeekEndingDate >= '2021-01-01' AND WeekEndingDate <= '2025-12-31' ORDER BY WeekEndingDate"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

$existingDates = [System.Collections.Generic.HashSet[string]]::new()
while ($r.Read()) {
    $d = [DateTime]$r["WeekEndingDate"]
    [void]$existingDates.Add($d.ToString("yyyy-MM-dd"))
}
$r.Close()
$connection.Close()

# Generate all Saturdays between Jan 1, 2021 and Dec 31, 2025
$startDate = Get-Date "2021-01-02"
# Find first Saturday in 2021
while ($startDate.DayOfWeek -ne [System.DayOfWeek]::Saturday) {
    $startDate = $startDate.AddDays(1)
}
$endDate = Get-Date "2025-12-31"

$missingSaturdays = @()
$curr = $startDate
while ($curr -le $endDate) {
    $dateStr = $curr.ToString("yyyy-MM-dd")
    if (-not $existingDates.Contains($dateStr)) {
        $missingSaturdays += $curr
    }
    $curr = $curr.AddDays(7)
}

Write-Host "Total Expected Saturdays (2021-2025):" ([Math]::Floor(($endDate - $startDate).Days / 7) + 1)
Write-Host "Total Existing Records (2021-2025):" $existingDates.Count
Write-Host "Total Missing Saturdays (2021-2025):" $missingSaturdays.Count
Write-Host "`nDetailed List of Missing Weeks (Saturdays):"
Write-Host "--------------------------------------------"
foreach ($m in $missingSaturdays) {
    Write-Host ("Year {0}: Week Ending {1:yyyy-MM-dd} ({2:MMMM d, yyyy})" -f $m.Year, $m, $m)
}
