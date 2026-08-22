$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT 
    YEAR(WeekEndingDate) as Yr, 
    COUNT(*) as Wks, 
    SUM(ABS(OnlineCommission) + ABS(InstantCommission)) as Comm, 
    SUM(ABS(OnlineCashBonus) + ABS(InstantCashBonus) + ABS(OnlineClaimsBonus) + ABS(InstantClaimsBonus)) as Bonus, 
    SUM(ABS(OnlineCommission) + ABS(InstantCommission) + ABS(OnlineCashBonus) + ABS(InstantCashBonus) + ABS(OnlineClaimsBonus) + ABS(InstantClaimsBonus)) as TotalEarnings,
    SUM(TotalDue) as TotalDue
FROM LotteryWeeklyStats 
GROUP BY YEAR(WeekEndingDate) 
ORDER BY Yr DESC
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

Write-Host "Year | Weeks | Commission  | Bonus      | Total Earnings | Total Due"
Write-Host "------------------------------------------------------------------"
while ($r.Read()) {
    Write-Host ("{0,4} | {1,5} | {2,11:C2} | {3,10:C2} | {4,14:C2} | {5,11:C2}" -f $r["Yr"], $r["Wks"], $r["Comm"], $r["Bonus"], $r["TotalEarnings"], $r["TotalDue"])
}
$r.Close()
$connection.Close()
