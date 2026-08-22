$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT 
    YEAR(ShiftDate) as Yr, 
    COUNT(*) as ShiftCount, 
    SUM(TotalSales) as TotalSales, 
    SUM(Commission) as Commission, 
    SUM(NetDue) as NetDue
FROM LotteryShifts 
GROUP BY YEAR(ShiftDate) 
ORDER BY Yr DESC
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

Write-Host "LotteryShifts Table (Mobile App & Daily Shift Entries):"
Write-Host "Year | Shift Count | Total Sales   | Commission  | Net Due"
Write-Host "------------------------------------------------------------------"
while ($r.Read()) {
    Write-Host ("{0,4} | {1,11} | {2,13:C2} | {3,11:C2} | {4,12:C2}" -f $r["Yr"], $r["ShiftCount"], $r["TotalSales"], $r["Commission"], $r["NetDue"])
}
$r.Close()
$connection.Close()
