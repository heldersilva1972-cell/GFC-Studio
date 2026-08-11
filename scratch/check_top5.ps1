$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT TOP 10 ShiftId, ShiftDate, ShiftType, EmployeeName, TotalSales, Status FROM dbo.LotteryShifts ORDER BY ShiftDate DESC"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

while ($r.Read()) {
    Write-Host ("ShiftId={0,-4} | Date={1:yyyy-MM-dd} | Shift={2,-5} | Employee='{3}' | Sales={4,7:C2} | Status='{5}'" -f $r["ShiftId"], $r["ShiftDate"], $r["ShiftType"], $r["EmployeeName"], $r["TotalSales"], $r["Status"])
}
$r.Close()
$connection.Close()
