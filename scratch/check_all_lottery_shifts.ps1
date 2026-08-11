$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT ShiftId, ShiftDate, ShiftType, EmployeeName, TotalSales, NetDue, Status, IsReconciled, CreatedBy, CreatedDate FROM LotteryShifts ORDER BY ShiftDate DESC, ShiftType"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

$count = 0
while ($r.Read()) {
    $count++
    Write-Host ("ShiftId={0,-4} | Date={1:yyyy-MM-dd} | Shift={2,-5} | Employee='{3}' | Sales={4,7:C2} | Status='{5}' | Reconciled={6}" -f $r["ShiftId"], $r["ShiftDate"], $r["ShiftType"], $r["EmployeeName"], $r["TotalSales"], $r["Status"], $r["IsReconciled"])
}
$r.Close()

Write-Host "=========================================================================="
Write-Host "Total LotteryShifts in DB: $count"

$connection.Close()
