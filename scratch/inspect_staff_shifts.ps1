$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=== StaffShifts for Sep 2026 ==="
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT * FROM StaffShifts WHERE ShiftDate >= '2026-09-15' OR ShiftStartTime >= '2026-09-15'"
$r = $cmd.ExecuteReader()
while ($r.Read()) {
    $row = @()
    for ($i=0; $i -lt $r.FieldCount; $i++) {
        $row += "$($r.GetName($i))=$($r.GetValue($i))"
    }
    Write-Host ($row -join ' | ')
}
$r.Close()

Write-Host "`n=== BarSaleEntries for Sep 15-18 ==="
$cmd2 = $conn.CreateCommand()
$cmd2.CommandText = "SELECT Id, SaleDate, Shift, EmployeeUsername, TotalSales, TotalHours, Status, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt FROM BarSaleEntries WHERE SaleDate >= '2026-09-15' ORDER BY SaleDate, Shift"
$r2 = $cmd2.ExecuteReader()
while ($r2.Read()) {
    Write-Host "ID: $($r2['Id']) | Date: $($r2['SaleDate']) | Shift: $($r2['Shift']) | User: $($r2['EmployeeUsername']) | Sales: $($r2['TotalSales']) | Hours: $($r2['TotalHours']) | Status: $($r2['Status']) | CreatedBy: $($r2['CreatedBy'])"
}
$r2.Close()

$conn.Close()
