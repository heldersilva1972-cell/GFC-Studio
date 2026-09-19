$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, SaleDate, Shift, EmployeeUsername, TotalSales, TotalHours, Status FROM BarSaleEntries WHERE Id = 3493"
$r = $cmd.ExecuteReader()
if ($r.Read()) {
    Write-Host "BarSaleEntry 3493 -> User: $($r['EmployeeUsername']) | Sales: $($r['TotalSales']) | Hours: $($r['TotalHours']) | Status: $($r['Status'])"
}
$r.Close()
$conn.Close()
